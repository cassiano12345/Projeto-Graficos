using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PdfSharp.Pdf;
using PdfSharp.Drawing;

namespace Graficos
{
    public partial class ChartDisplay : Form
    {

        string codemp;
        string codutil;


        private String[] args;
        ToolTip tooltip;
        int chartColor = 0;
        private ToolTip toolTip;
        string codempp, codut, opcao, nrvd, trimestre, mes, conta;
        int chartType = 0, ano, x=0, cor=0; //0 - Line ; 1 - Columns
        bool Linhas, barras, pie;
        Series series1, series2, series3;
        Series series4, series5, series6, series = new Series("PieSeries");
        List<List<KeyValuePair<String, Double>>> listKVP = new List<List<KeyValuePair<string, Double>>>();
        private string additionalInfo;
        List<string> minhaLista = new List<string>();
        private List<string> allGraficos = new List<string>(); // Lista de todas as imagens
        private List<string> activeGraficos = new List<string>(); // Lista de imagens ativas
        private int currentImageIndex = 0; // Índice da imagem atual
        private List<Image> allImages = new List<Image>(); // Lista de todas as imagens
        private List<Image> activeImages = new List<Image>(); // Lista de imagens ativas


        public ChartDisplay()
        {
            InitializeComponent();
            LoadGraficos();
            imagem();
            
            args = new String[Environment.GetCommandLineArgs().Length];
            args = Environment.GetCommandLineArgs();
            tooltip = new ToolTip();

            //Argumentos
            if (args.Length == 1)
                Close();

            if (args.Length >= 3)
                codempp = args[2]; // Argumento CODEMP

            if (args.Length >= 4)
                codut = args[3]; // Argumento CODUTIL

            if (args.Length >= 5)
                ano = int.Parse(args[4]); // Argumento ANO

            if (args.Length >= 6) //Argumento Opção
                opcao = args[5];

            if (args.Length >= 7)
            {
                nrvd = args[6];
            } //Argumento Trimestre

            if (args.Length >= 8) //Argumento 
                trimestre = args[7];

            if (args.Length >= 9) //Argumento mes
                mes = args[8];

            if (args.Length >= 10) //Argumento conta
                conta = args[9];
            //Argumentos

            //this.FormBorderStyle = FormBorderStyle.FixedDialog; // Remove o botão maximizar
            //this.MaximizeBox = false; // Remove o botão maximizar
            //this.MinimizeBox = false; // Remove o botão minimizar
        }

        private void LoadGraficos() // Função para adicionar o nome dos graficos e imagem do botão nas listas
        {
            // Carrega todas as imagens da lista de recursos
            allGraficos.Add("Linhas");
            allGraficos.Add("Barras");
            allGraficos.Add("Pie");
            allImages.Add(Graficos.Properties.Resources.chart_Lines);
            allImages.Add(Graficos.Properties.Resources.chart_Columns);
            allImages.Add(Graficos.Properties.Resources.chart_Pie);

        }

        private void imagem() //Função para obter o index do grafico atual
        {
            if (activeGraficos.Count == 0) return;
            // Muda para a próxima imagem
            currentImageIndex = (currentImageIndex + 1) % activeGraficos.Count;
            UpdateGrafico();
        }


        private void UpdateGrafico()
        {
            if (x == 1)
            {
                foreach (Series series in chart1.Series)
                {
                    series.Enabled = true;
                }
                chart1.Series["PieSeries"].Enabled = false;
                // Atualizar o gráfico
                chart1.Invalidate();
            }


            if (activeGraficos[currentImageIndex] == "Linhas") // Condição para ativar o grafico de linhas
            {
                // Columns -> Line
                foreach (var series in chart1.Series)
                {
                    series.ChartType = SeriesChartType.Line;
                }
                if (activeGraficos.Count == 1)
                {
                    button1.Image = activeImages[currentImageIndex ];
                }
                else 
                {
                    button1.Image = activeImages[currentImageIndex + 1];
                }
            }
            else if (activeGraficos[currentImageIndex] == "Barras" ) // Condição para ativar o grafico de Barras
            {
                // Line -> Columns
                foreach (var series in chart1.Series)
                {
                    series.ChartType = SeriesChartType.Column;
                    //chart2.ChartAreas
                }
               if(activeGraficos.Count == 1)
                {
                    button1.Image = activeImages[currentImageIndex];
                } else if(activeGraficos.Count == 2 && currentImageIndex == 0)
                {
                    button1.Image = activeImages[currentImageIndex + 1];

                }
                else if (activeGraficos.Count == 2 && currentImageIndex == 1)
                {
                    button1.Image = activeImages[(currentImageIndex + 1) - activeGraficos.Count];

                }
                else {
                    button1.Image = activeImages[currentImageIndex + 1]; 
                }
            }
            else if (activeGraficos[currentImageIndex] == "Pie") // Condição para ativar o grafico de Queijo
            {
                if (minhaLista.Count() < 2) {
                    foreach (var series in chart1.Series)
                    {
                        series.ChartType = SeriesChartType.Pie;
                        //chart2.ChartAreas
                    }
                } else { 
                if (x == 0)
                {
                    foreach (Series seriess in chart1.Series)
                    {
                        seriess.Enabled = false;
                    }
                    // Atualizar o gráfico
                    chart1.Invalidate();
                    // Criar uma nova série para o gráfico de pizza
                    Series series = new Series("PieSeries");
                    series.ChartType = SeriesChartType.Pie;

                    // Índice para percorrer minhaLista
                    int i = 0;

                    // Iterar sobre cada lista interna (cada série de dados)
                    foreach (var dataSeries in listKVP)
                    {
                        // Somar os valores para esta série
                        double sum = 0;
                        foreach (var kvp in dataSeries)
                        {
                            sum += kvp.Value;
                        }

                        // Adicionar o valor total ao gráfico de pizza
                        // Usar minhaLista[i] como nome da fatia
                        series.Points.AddXY(minhaLista[i], sum);

                        // Incrementar o índice
                        i++;
                    }
                    // Adicionar a série ao gráfico
                    chart1.Series.Add(series); x = 1;

                }
                else
                {
                    foreach (Series seriess in chart1.Series)
                    {
                        seriess.Enabled = false;
                    }
                    chart1.Series["PieSeries"].Enabled = true;
                    ChangeChartType(SeriesChartType.Pie);

                    // Atualizar o gráfico
                    chart1.Invalidate();
                }
               
            }
                if (activeGraficos.Count == 1)
                {
                    button1.Image = activeImages[currentImageIndex];
                }
                else
                {
                    button1.Image = activeImages[(currentImageIndex + 1) - activeGraficos.Count];
                }
               
            }
        }

        private void GerarGraficoOFI008()
        {
            chart1.Titles.Add("Análise de Vendas: " + ano);
            minhaLista.Add("Vendas");
            listKVP.Add(GraficosDataBase.getGraficoOFI008(codempp, codut, ano));
            GerarGrafico("MESES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA015_A()
        {
            chart1.Titles.Add("Análise de Vendas");
            minhaLista.Add("Ano");
            minhaLista.Add("Ano - 1");
            minhaLista.Add("Ano - 2");
            listKVP = GraficosDataBase.getGraficoNOVA015_A(codempp, codut, ano);
            GerarGrafico("MESES", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA015_BM()
        {
            chart1.Titles.Add("Saldos de Contas - Movimentos Mensais");
            minhaLista.Add("Saldo Ano");
            minhaLista.Add("Saldo Ano 1");
            minhaLista.Add("Saldo Ano 2");
            listKVP = GraficosDataBase.getGraficoNOVA015_BM(codempp, codut);
            GerarGrafico("MESES", "Valor €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA015_BA()
        {

            chart1.Titles.Add("Saldos de Contas - Acumulados");
            minhaLista.Add("Saldo Ano");
            minhaLista.Add("Saldo Ano 1");
            minhaLista.Add("Saldo Ano 2");
            listKVP = GraficosDataBase.getGraficoNOVA015_BA(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoOBR002()
        {
            chart1.Titles.Add("Recebimento de Obras");
            minhaLista.Add("Recebimento de Obras");// Criar uma nova serie
            listKVP.Add(GraficosDataBase.getGraficoOBR002(codempp, codut));
            GerarGrafico("ESTADOS", "VALORES", 3, 1, 1);
            //activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            //activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoOBR013_AC()
        {
           chart1.Titles.Add("Acumulados Mensais");
            minhaLista.Add("Equipamento");// Criar uma nova serie
            minhaLista.Add("Mão Obra");// Criar uma nova serie
            minhaLista.Add("T. Custos");// Criar uma nova serie
            minhaLista.Add("SubEmp.");// Criar uma nova serie
            minhaLista.Add("Materiais");// Criar uma nova serie
            minhaLista.Add("Tot.Custos");// Criar uma nova serie
            listKVP = GraficosDataBase.getGraficoOBR013_AC(codempp, codut, opcao);
            GerarGrafico("MES / ANO", "VALORES", 3, 1, 6);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            //activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            //activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoOBR013_FT()
        {

            chart1.Titles.Add("Recebimento de Obras");
            minhaLista.Add("Valores");// Criar uma nova serie
            listKVP.Add(GraficosDataBase.getGraficoOBR013_FT(codempp, codut, opcao));
            GerarGrafico("DADOS", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }












        private void GerarGraficoOBR013_CT()
        {
            chart1.Titles.Add("Custos de Obra");
            minhaLista.Add("Custos"); // Criar uma nova serie
            minhaLista.Add("Mão de Obra"); // Criar uma nova serie
            minhaLista.Add("T. Custos"); // Criar uma nova serie
            minhaLista.Add("Sub-Emp"); // Criar uma nova serie
            minhaLista.Add("Materiais"); // Criar uma nova serie
            listKVP = GraficosDataBase.getGraficoOBR013_CT(codempp, codut, opcao);
            GerarGrafico("Tipo de custos", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }




        /*
        private void GerarGraficoOBR013_CT()
        {
            chart1.Titles.Add("Custos de Obra");
            minhaLista.Add("Custos"); // Criar uma nova serie
            minhaLista.Add("Mão de Obra"); // Criar uma nova serie
            minhaLista.Add("T. Custos"); // Criar uma nova serie
            minhaLista.Add("Sub-Emp"); // Criar uma nova serie
            minhaLista.Add("Materiais"); // Criar uma nova serie
            listKVP = GraficosDataBase.getGraficoOBR013_CT(codempp, codut, opcao);
            GerarGrafico("Tipo de custos", "VALORES €", 3, "Valores 1,2,3...", 1, 1);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        */









        private void GerarGraficoCRM008_CT()
{
            chart1.Titles.Add("Custos " + args[7] + "º Trimestre");
            minhaLista.Add("Alimentação Previsão");// Criar uma nova serie
            minhaLista.Add("Alimentação Real");// Criar uma nova serie
            minhaLista.Add("Combustível Alimentação");// Criar uma nova serie
            minhaLista.Add("Combustível Real");// Criar uma nova serie
            listKVP = (GraficosDataBase.getGraficoCRM008_CT(codempp, nrvd, trimestre));
            GerarGrafico("MESES", "VALORES €", 3, 1, 4);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoCRM008_FT()
        {
            minhaLista.Add("Desvio");// Criar uma nova serie
            chart1.Titles.Add("Resultados");
            listKVP.Add(GraficosDataBase.getGraficoCRM008_FT(codempp));
            GerarGrafico("NOMES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            //activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            //activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoCRM008_VS()
{
            minhaLista.Add("Visitas");// Criar uma nova serie
            minhaLista.Add("Vendas");// Criar uma nova serie
            chart1.Titles.Add("Acordos Comerciais e Visitas de " + GraficosDataBase.getFullMonthName(mes));
            listKVP = GraficosDataBase.getGraficoCRM008_VS(codempp);
            GerarGrafico("VENDEDORES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoCRM008_RS1()
{
            minhaLista.Add("Vendas");// Criar uma nova serie
            minhaLista.Add("Despesas");// Criar uma nova serie
            minhaLista.Add("Margem");// Criar uma nova serie
            chart1.Titles.Add("Resultado");
            listKVP = GraficosDataBase.getGraficoCRM008_RS1(codempp);
            GerarGrafico("ANO", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoCRM008_RS2()
{
            chart1.Titles.Add("Resultados " + args[4].ToString());
            minhaLista.Add("Valores");// Criar uma nova serie
            listKVP.Add(GraficosDataBase.getGraficoCRM008_RS2(codempp));
            GerarGrafico("DADOS", "VALORES €", 3, 1, 1);
            //activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            //activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoMAN006_C()
        {
            minhaLista.Add("Curativa");// Criar uma nova serie
            minhaLista.Add("Preventiva");// Criar uma nova serie
            chart1.Titles.Add("Custos Mensais " + args[4]);
            listKVP = (GraficosDataBase.getGraficoMAN006_C(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoMAN006_T()
        {
            chart1.Titles.Add("Total Custos Mensais " + args[4]);
            minhaLista.Add("Total");// Criar uma nova serie// Criar uma nova serie
            //series2.ChartType = SeriesChartType.Line;
            listKVP = (GraficosDataBase.getGraficoMAN006_T(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoTes002_DC()
        {
        chart1.Titles.Add("Disponibilidades - Caixa " + args[4]);
            minhaLista.Add("Entradas");// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add("Saídas");// Criar uma nova serie// Criar uma nova serie
            listKVP = (GraficosDataBase.getGraficoTes002_DC(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_DB()
        {
            minhaLista.Add("Entradas");// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add("Saídas");// Criar uma nova serie// Criar uma nova serie
            chart1.Titles.Add("Disponibilidades - Banco " + args[4]);
            listKVP = (GraficosDataBase.getGraficoTes002_DB(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_GV()
        {
            minhaLista.Add("Mensais");// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add("Acumulativas");// Criar uma nova serie// Criar uma nova serie
            chart1.Titles.Add("Vendas");
            listKVP = (GraficosDataBase.getGraficoTes002_GV(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_GC()
        {
            minhaLista.Add("Mensais");// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add("Acumulativas");// Criar uma nova serie// Criar uma nova serie
            chart1.Titles.Add("Compras");
            listKVP = (GraficosDataBase.getGraficoTes002_GC(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_LU()
        {
            minhaLista.Add("Lucro");// Criar uma nova serie// Criar uma nova serie
            chart1.Titles.Add("Lucro Bruto - " + args[4].ToString());
            listKVP.Add(GraficosDataBase.getGraficoTes002_LU(codempp, codut));
            GerarGrafico("MESES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_CU()
        {
            minhaLista.Add(args[4].ToString());// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add((int.Parse(args[4].ToString()) - 1).ToString());// Criar uma nova serie// Criar uma nova serie
            minhaLista.Add((int.Parse(args[4].ToString()) - 2).ToString());// Criar uma nova serie// Criar uma nova serie
            chart1.Titles.Add("Custos");
            listKVP = (GraficosDataBase.getGraficoTes002_CU(codempp, codut));
            GerarGrafico("CONTA", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoTes002_PR()
        {
            minhaLista.Add(args[4].ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 1).ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 2).ToString());
            chart1.Titles.Add("Proveitos");
            listKVP = (GraficosDataBase.getGraficoTes002_PR(codempp, codut));
            GerarGrafico("CONTA", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_RC()
        {
            minhaLista.Add("Saldo");
            chart1.Titles.Add(args[9].ToString() + " " + getNomeConta(args[9]) + " " + args[4]);
            listKVP.Add(GraficosDataBase.getGraficoTes002_RC(codempp, codut, conta));
            GerarGrafico("MESES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_CF()
        {
            minhaLista.Add(args[4].ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 1).ToString());
            listKVP = GraficosDataBase.getGraficoTes002_CF(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_RP()
        {
            minhaLista.Add("Recibos");
            minhaLista.Add("Pagamentos");
            chart1.Titles.Add(" Recibos / Pagamentos - " + DateTime.Now.ToShortDateString());
            listKVP = GraficosDataBase.getGraficoTes002_RP(codempp, codut);
            GerarGrafico("DATA", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoTes002_TS()
        {
            minhaLista.Add("Disp.");
            minhaLista.Add("Pagam.");
            minhaLista.Add("Saldo");
            chart1.Titles.Add("Previsão de Tesouraria - " + DateTime.Now.ToShortDateString());
            listKVP = GraficosDataBase.getGraficoTes002_TS(codempp, codut);
            GerarGrafico("DATA", "VALORES €", 3, 1, 3);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }
        
        private void GerarGraficoImb004()
        {
            minhaLista.Add(args[4]);
            minhaLista.Add((int.Parse(args[4].ToString()) - 1).ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 2).ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 3).ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 4).ToString());
            minhaLista.Add((int.Parse(args[4].ToString()) - 5).ToString());
            
            chart1.Titles.Add("Análise de Valores");
            listKVP = GraficosDataBase.getGraficoImb004(codempp, codut, ano);
            GerarGrafico("ANOS", "VALORES €", 3, 0, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficogps012()
        {
            minhaLista.Add("Valores(Euros)");
            chart1.Titles.Add("Análise de Valores");
            chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;         // Apresentar todos os dados no Eixo de X
            listKVP.Add(GraficosDataBase.getGraficogps012(codempp, codut, ano, opcao));
            GerarGrafico("ANOS", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoNOVA014_E001()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Facturação");
            listKVP = GraficosDataBase.getGraficoNOVA014_E001(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoNOVA014_E002()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Facturação - Anual");
            listKVP = GraficosDataBase.getGraficoNOVA014_E002(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoNOVA014_E003()
        {
            minhaLista.Add("Dados");
            chart1.Titles.Add("Vendas por vendedor");
            listKVP = GraficosDataBase.getGraficoNOVA014_E003(codempp, codut);
            GerarGrafico("VENDEDORES", "VALORES €", 3, 1, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        
        private void GerarGraficoNOVA014_E004()
        {
            minhaLista.Add("Valores");
            chart1.Titles.Add("Vendas por vendedor");
            listKVP = GraficosDataBase.getGraficoNOVA014_E004(codempp, codut);
            GerarGrafico("VENDEDORES", "VALORES €", 3, 1, 1);
            //activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            //activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoNOVA014_E005()
        {
            minhaLista.Add( DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Responsabilidade mensal");
            listKVP = GraficosDataBase.getGraficoNOVA014_E001(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }
        
        private void GerarGraficoNOVA014_C001()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Responsabilidade mensal");
            listKVP = GraficosDataBase.getGraficoNOVA014_C001(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA014_C002()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Responsabilidade acumulativa");
            listKVP = GraficosDataBase.getGraficoNOVA014_C002(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA014_C003()
        {
            minhaLista.Add("Proveitos");
            minhaLista.Add("Custos");
            chart1.Titles.Add("Custos e Proveitos Mensais");
            listKVP = GraficosDataBase.getGraficoNOVA014_C003(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA014_C004()
        {
            minhaLista.Add("Proveitos");
            minhaLista.Add("Custos");
            chart1.Titles.Add("Custos e Proveitos Evolução");
            listKVP = GraficosDataBase.getGraficoNOVA014_E002(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA014_C005()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Resultado, Evolução");
            listKVP = GraficosDataBase.getGraficoNOVA014_C005(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoNOVA014_C006()
        {
            minhaLista.Add(DateTime.Today.AddYears(-1).Year.ToString());
            minhaLista.Add(DateTime.Now.Year.ToString());
            chart1.Titles.Add("Analise Débitos vendedor/mensal");
            listKVP = GraficosDataBase.getGraficoNOVA014_C006(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }

        private void GerarGraficoGR001()
        {
            minhaLista.Add("Valores");
            chart1.Titles.Add("Vendas Mensais Gerais 12M");
            listKVP = GraficosDataBase.getGraficoGR001(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 2, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoGR003()
        {
            minhaLista.Add("Valores");
            chart1.Titles.Add("Vendas Anuais 12A");
            listKVP = GraficosDataBase.getGraficoGR003(codempp, codut);
            GerarGrafico("ANOS", "VALORES €", 3, 2, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoGR004()
        {
            minhaLista.Add("Valores");
            chart1.Titles.Add("Vendas Diárias 31D");
            listKVP = GraficosDataBase.getGraficoGR004(codempp, codut);
            GerarGrafico("DATA", "VALORES €", 3, 2, 1);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoGR005()
        {
            minhaLista.Add("Vendas");
            minhaLista.Add("Margem");
            chart1.Titles.Add("Margens Mensais 12M");
            listKVP = GraficosDataBase.getGraficoGR005(codempp, codut);
            GerarGrafico("MESES", "VALORES €", 3, 1, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }


        private void GerarGraficoGR006()
        {
            minhaLista.Add((int.Parse(args[4].ToString()) - 1).ToString());
            minhaLista.Add(args[4]);
            chart1.Titles.Add("Vendas Comparativas 12M");
            listKVP = GraficosDataBase.getGraficoGR006(codempp, codut);
            GerarGrafico("MESES", "VALORES", 3, 2, 2);
            activeGraficos.Add(allGraficos[0]);
            activeGraficos.Add(allGraficos[1]);
            activeImages.Clear();
            activeImages.Add(allImages[0]);
            activeImages.Add(allImages[1]);
            UpdateGrafico();
        }

        private void GerarGraficoGR007()
        {
            minhaLista.Add(args[4]);
            chart1.Titles.Add("Vendas por Zona de " + DateTime.Today.AddYears(-1).ToShortDateString() + " a " + DateTime.Today.ToShortDateString() + "");
            listKVP = GraficosDataBase.getGraficoGR007(codempp, codut);
            GerarGrafico("ZONAS", "VALORES", 3, 1, 1);
            activeGraficos.Add(allGraficos[2]);
            activeImages.Clear();
            activeImages.Add(allImages[2]);
            UpdateGrafico();
        }


        private void ChangeChartType(SeriesChartType chartType)
        {
            // Mudando o tipo de gráfico da série
            foreach (var series in chart1.Series)
            {
                series.ChartType = chartType;
            }
        }

        static Image MergeImages(Image img1)
        {
            var finalSize = new Size();
            finalSize.Width = img1.Width+50;
            finalSize.Height = img1.Height+90;

            var outputImage = new Bitmap(finalSize.Width, finalSize.Height);
            using (var gfx = Graphics.FromImage(outputImage))
            {
                gfx.Clear(Color.White);
                gfx.DrawImage(img1, 20, 50);
                gfx.DrawIcon(Graficos.Properties.Resources.NOVA, new Rectangle(20, 10, 32, 32));
                gfx.DrawString("Nov@Gest", new Font("Arial", 13, FontStyle.Bold), Brushes.Gray, new PointF(50, 16));
                gfx.DrawString("Gerado a " + DateTime.Now.ToString("D") + " às " + DateTime.Now.ToString("t"), new Font("Arial", 9, FontStyle.Regular), Brushes.Black, new PointF(20, finalSize.Height - 20));
            }
            return outputImage;
        }


        //Funções do Form

        private void button1_Click(object sender, EventArgs e)
        {
            imagem();
        }

        private void chart_MouseMove(object sender, MouseEventArgs e)
        {
            Chart chart = sender as Chart;
            HitTestResult result = chart.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                DataPoint point = result.Series.Points[result.PointIndex];
                if (result.Series.ChartType == SeriesChartType.Line)
                {
                    //Line
                    //double yValue = chart.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);
                    //tooltip.Show($"{result.Series.Name} (€): {yValue:F2}", chart, e.X, e.Y - 15);

                    ChartArea area = chart.ChartAreas[result.Series.ChartArea];
                    double yValue = area.AxisY.PixelPositionToValue(e.Y); // Converte a posição do pixel para o valor do eixo Y
                    tooltip.Show($"{result.Series.Name} (€): {yValue:F2}", chart, e.X, e.Y - 15);
                }
                else if (result.Series.ChartType == SeriesChartType.Pie)
                {
                    //Pie
                    string label = point.AxisLabel; // Obtém o rótulo da fatia
                    double yValue = point.YValues[0]; // Obtém o valor da fatia
                    tooltip.Show($"{label} (€): {yValue:F2}", chart, e.X, e.Y - 15);
                }
                else
                {
                    //Columns
                    double yValue = point.YValues[0];
                    tooltip.Show($"{result.Series.Name} (€): {yValue:F2}", chart, e.X, e.Y - 15);
                }
            }
            else
            {
                tooltip.Hide(chart);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Exportar PDF
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Arquivos PDF (*.pdf)|*.pdf",
                Title = "Salvar Gráfico como PDF"
            };
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                PdfDocument document = new PdfDocument();
                try {

                    //Primeira Página
                    PdfPage page = document.AddPage();
                    XGraphics gfx = XGraphics.FromPdfPage(page);

                    // Desenha o ícone e o texto no topo
                    using (MemoryStream iconStream = new MemoryStream())
                    {
                        Image icoImg = new Icon(Properties.Resources.NOVA, 128, 128).ToBitmap();
                        icoImg.Save(iconStream, System.Drawing.Imaging.ImageFormat.Png);
                        iconStream.Seek(0, SeekOrigin.Begin);
                        XImage iconImage = XImage.FromStream(iconStream);
                        gfx.DrawImage(iconImage, 10, 10, 32, 32);

                        gfx.DrawString("Nov@Gest", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Gray, 40, 32);
                    }


                    double yPos = 100;
                        using (MemoryStream stream = new MemoryStream())
                        {
                            chart1.SaveImage(stream, ChartImageFormat.Png);
                            stream.Seek(0, SeekOrigin.Begin);
                            XImage image = XImage.FromStream(stream);

                            double aspectRatio = (double)image.PixelWidth / image.PixelHeight;
                            double imageWidth = page.Width - 20;
                            double imageHeight = imageWidth / aspectRatio;

                            gfx.DrawImage(image, 10, yPos, imageWidth, imageHeight);
                            yPos += imageHeight;
                        }
                    

                    gfx.DrawString("Gerado a " + DateTime.Now.ToString("D") + " às " + DateTime.Now.ToString("t"), new XFont("Arial", 9, XFontStyle.Regular), XBrushes.Black, 10, yPos + 50);

                    document.Save(saveFileDialog.FileName);
                    document.Close();

                    DialogResult dresult = MessageBox.Show("Arquivo gerado com Sucesso!\nAbrir ficheiro?", "Exportar para PDF", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dresult == DialogResult.Yes) System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao gerar Arquivo! (Erro: " + ex.Message + " )", "Exportar para PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Exportar JPG
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "Arquivos de Imagem (*.jpg)|*.jpg";
            saveFileDialog.Filter = "Arquivos de Imagem (*.jpg)|*.jpg";
            saveFileDialog.Title = "Salvar Gráfico como Imagem";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "")
            {
                try {
                    Image bmp1;
                    using (var ms = new MemoryStream())
                    {
                        chart1.SaveImage(ms, ChartImageFormat.Jpeg);
                        bmp1 = System.Drawing.Bitmap.FromStream(ms);
                    }
                   
                    var finalImage = MergeImages(bmp1);
                    finalImage.Save(saveFileDialog.FileName, ImageFormat.Jpeg);

                    DialogResult dresult = MessageBox.Show("Arquivo gerado com Sucesso!\nAbrir ficheiro?", "Exportar para JPG", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dresult == DialogResult.Yes) System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Erro ao gerar Arquivo! (Erro: "+ex.Message+" )", "Exportar para JPG", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            chartColor++;
            switch (chartColor)
            { //blue red green yellow purple pink cyan
                case 1:
                    chart1.Series[minhaLista[0+cor]].Color = System.Drawing.Color.Red;
                    

                    break;
                case 2:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Green;
                    
                    break;
                case 3:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Yellow;
                    
                    break;
                case 4:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Purple;
                    
                    break;
                case 5:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Pink;
                   
                    break;
                case 6:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Cyan;
                    
                    break;
                default:
                    chart1.Series[minhaLista[0+ cor]].Color = System.Drawing.Color.Blue;
                    if (cor < minhaLista.Count)
                        cor++;
                    if (cor == minhaLista.Count)
                        cor=0;
                    chartColor = 0;
                    break;
            }
            
        }


        private void button4_Click_1(object sender, EventArgs e)
        {
            if (x == 1)
            {
                foreach (Series series in chart1.Series)
                {
                    series.Enabled = true;
                }
                chart1.Series["PieSeries"].Enabled = false;
                // Atualizar o gráfico
                chart1.Invalidate();
            }

            foreach (var series in chart1.Series)
            {
                series.ChartType = SeriesChartType.Column;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (x == 0)
            {
                foreach (Series seriess in chart1.Series)
                {
                    seriess.Enabled = false;
                }

                // Atualizar o gráfico
                chart1.Invalidate();
                // Criar uma nova série para o gráfico de pizza
                Series series = new Series("PieSeries");
                series.ChartType = SeriesChartType.Pie;

                // Índice para percorrer minhaLista
                int i = 0;

                // Iterar sobre cada lista interna (cada série de dados)
                foreach (var dataSeries in listKVP)
                {
                    // Somar os valores para esta série
                    double sum = 0;
                    foreach (var kvp in dataSeries)
                    {
                        sum += kvp.Value;
                    }

                    // Adicionar o valor total ao gráfico de pizza
                    // Usar minhaLista[i] como nome da fatia
                    series.Points.AddXY(minhaLista[i], sum);

                    // Incrementar o índice
                    i++;
                }
                // Adicionar a série ao gráfico
                chart1.Series.Add(series); x = 1;

            }
            else
            {
                foreach (Series seriess in chart1.Series)
                {
                    seriess.Enabled = false;
                }
                chart1.Series["PieSeries"].Enabled = true;
                ChangeChartType(SeriesChartType.Pie);

                // Atualizar o gráfico
                chart1.Invalidate();
            }
            button1.Image = Graficos.Properties.Resources.chart_Lines;
            chartType = 2;
        }


        private void GerarGrafico(string Axisx, string Axisy, int borderW, int AxisX_int, int num_series)
        {
            //imagem_botão();
            if (num_series == 1)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X

                foreach (var kvpList in listKVP)
                {

                    foreach (var kvp in kvpList)
                    {
                        chart1.Series[minhaLista[0]].Points.AddXY(kvp.Key, kvp.Value);
                    }
                }
            }

            if (num_series == 2)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                series2 = new Series(minhaLista[1]);
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                this.chart1.Series.Add(series2);
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.Series[minhaLista[1]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X

                foreach (var kvpList in listKVP[0])
                {
                    chart1.Series[minhaLista[0]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[1])
                {
                    chart1.Series[minhaLista[1]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
            }
            if (num_series == 3)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                series2 = new Series(minhaLista[1]);
                series3 = new Series(minhaLista[2]);
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                this.chart1.Series.Add(series2);
                this.chart1.Series.Add(series3);
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.Series[minhaLista[1]].BorderWidth = borderW;
                chart1.Series[minhaLista[2]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X
                series1.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series2.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series3.ChartType = SeriesChartType.Line;

                foreach (var kvpList in listKVP[0])
                {
                    chart1.Series[minhaLista[0]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[1])
                {
                    chart1.Series[minhaLista[1]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[2])
                {
                    chart1.Series[minhaLista[2]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
            }

            if (num_series == 4)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                series2 = new Series(minhaLista[1]);
                series3 = new Series(minhaLista[2]);
                series4 = new Series(minhaLista[3]);
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                this.chart1.Series.Add(series2);
                this.chart1.Series.Add(series3);
                this.chart1.Series.Add(series4);
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.Series[minhaLista[1]].BorderWidth = borderW;
                chart1.Series[minhaLista[2]].BorderWidth = borderW;
                chart1.Series[minhaLista[3]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X
                series1.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series2.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series3.ChartType = SeriesChartType.Line;
                series4.ChartType = SeriesChartType.Line;

                foreach (var kvpList in listKVP[0])
                {
                    chart1.Series[minhaLista[0]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[1])
                {
                    chart1.Series[minhaLista[1]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[2])
                {
                    chart1.Series[minhaLista[2]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[3])
                {
                    chart1.Series[minhaLista[3]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
            }
            if (num_series == 5)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                series2 = new Series(minhaLista[1]);
                series3 = new Series(minhaLista[2]);
                series4 = new Series(minhaLista[3]);
                series5 = new Series(minhaLista[4]);
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                this.chart1.Series.Add(series2);
                this.chart1.Series.Add(series3);
                this.chart1.Series.Add(series4);
                this.chart1.Series.Add(series5);
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.Series[minhaLista[1]].BorderWidth = borderW;
                chart1.Series[minhaLista[2]].BorderWidth = borderW;
                chart1.Series[minhaLista[3]].BorderWidth = borderW;
                chart1.Series[minhaLista[4]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X
                series1.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series2.ChartType = SeriesChartType.Line;             // Criar uma nova serie
                series3.ChartType = SeriesChartType.Line;
                series4.ChartType = SeriesChartType.Line;
                series5.ChartType = SeriesChartType.Line;

                foreach (var kvpList in listKVP[0])
                {
                    chart1.Series[minhaLista[0]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[1])
                {
                    chart1.Series[minhaLista[1]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[2])
                {
                    chart1.Series[minhaLista[2]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[3])
                {
                    chart1.Series[minhaLista[3]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
                foreach (var kvpList in listKVP[4])
                {
                    chart1.Series[minhaLista[4]].Points.AddXY(kvpList.Key, kvpList.Value);
                }
            }
            if (num_series == 6)
            {
                chart1.ChartAreas["ChartArea1"].AxisX.Title = Axisx;
                chart1.ChartAreas["ChartArea1"].AxisY.Title = Axisy;
                series1 = new Series(minhaLista[0]);                 // Criar uma nova serie
                series2 = new Series(minhaLista[1]);
                series3 = new Series(minhaLista[2]);
                series4 = new Series(minhaLista[3]);
                series5 = new Series(minhaLista[4]);
                series6 = new Series(minhaLista[5]);
                this.chart1.Series.Add(series1);                      // Adicionar a serie ao grafico
                this.chart1.Series.Add(series2);
                this.chart1.Series.Add(series3);
                this.chart1.Series.Add(series4);
                this.chart1.Series.Add(series5);
                this.chart1.Series.Add(series6);
                chart1.Series[minhaLista[0]].BorderWidth = borderW;
                chart1.Series[minhaLista[1]].BorderWidth = borderW;
                chart1.Series[minhaLista[2]].BorderWidth = borderW;
                chart1.Series[minhaLista[3]].BorderWidth = borderW;
                chart1.Series[minhaLista[4]].BorderWidth = borderW;
                chart1.Series[minhaLista[5]].BorderWidth = borderW;
                chart1.ChartAreas["ChartArea1"].AxisX.Interval = AxisX_int;         // Apresentar todos os dados no Eixo de X
                
                foreach (var kvpList in listKVP[0])
                {
                    chart1.Series[minhaLista[0]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[1])
                {
                    chart1.Series[minhaLista[1]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[2])
                {
                    chart1.Series[minhaLista[2]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[3])
                {
                    chart1.Series[minhaLista[3]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[4])
                {
                    chart1.Series[minhaLista[4]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
                foreach (var kvpList in listKVP[5])
                {
                    chart1.Series[minhaLista[5]].Points.AddXY(kvpList.Key, kvpList.Value);

                }
            }
        }

        String getNomeConta(String conta)
        {
            switch (conta)
            {
                case "61":
                    return "C.M.V.M.C.";
                case "62":
                    return "Fornecimentos Serviços Externos";
                case "63":
                    return "Impostos";
                case "64":
                    return "Custos com Pessoal";
                case "65":
                    return "Outros Custos Operacionais";
                case "66":
                    return "Amortizações";
                case "67":
                    return "Provisões";
                case "68":
                    return "Custos e Perdas Financeiras";
                case "69":
                    return "Custos e Perdas Extraordinárias";
                case "71":
                    return "Vendas";
                case "72":
                    return "Prestações de Serviços";
                case "73":
                    return "Prestações Suplementares";
                case "74":
                    return "Subsídios à Exploração";
                case "75":
                    return "Trabalhos para a Empresa";
                case "76":
                    return "Outros Proveitos Operacionais";
                case "77":
                    return "...";
                case "78":
                    return "Proveitos e Ganhos Financeiros";
                case "79":
                    return "Proveitos e Ganhos Extraordinários";
                default:
                    return "";
            }


        }

        private void ChartDisplay_Load(object sender, EventArgs e)
        {
            escolher_grafico();
        }
        private void escolher_grafico()
        {
            switch (args[1])
            {
                case "OFI008":
                    GerarGraficoOFI008();
                    break;
                case "NOVA015_A":
                    GerarGraficoNOVA015_A();
                    break;
                case "NOVA015_BM":
                    GerarGraficoNOVA015_BM();
                    break;
                case "NOVA015_BA":
                    GerarGraficoNOVA015_BA();
                    break;
                case "OBR002":
                    GerarGraficoOBR002();
                    break;
                case "OBR013_AC":
                    GerarGraficoOBR013_AC();
                    break;
                
                case "OBR013_FT":
                GerarGraficoOBR013_FT();
                    break;
                
                case "OBR013_CT":
                GerarGraficoOBR013_CT();
                    break;
                
                case "CRM008_CT":
                GerarGraficoCRM008_CT();
                    break;
                
                case "CRM008_FT":
                GerarGraficoCRM008_FT();
                    break;
                
                case "CRM008_VS":
                GerarGraficoCRM008_VS();
                    break;
                
                case "CRM008_RS1":
                GerarGraficoCRM008_RS1();
                    break;
                
                case "CRM008_RS2":
                GerarGraficoCRM008_RS2();
                    break;
                
                case "MAN006_C":
                GerarGraficoMAN006_C();
                    break;
                
                case "MAN006_T":
                GerarGraficoMAN006_T();
                    break;
                
                case "TES002_DC":
                GerarGraficoTes002_DC();
                    break;
                
                case "TES002_DB":
                GerarGraficoTes002_DB();
                    break;
                
                case "TES002_GV":
                GerarGraficoTes002_GV();
                    break;
                
                case "TES002_GC":
                GerarGraficoTes002_GC();
                    break;
               
                case "TES002_LU":
                GerarGraficoTes002_LU();
                    break;
                   
                case "TES002_CU":
                GerarGraficoTes002_CU();
                    break;
                 
                case "TES002_PR":
                GerarGraficoTes002_PR();
                    break;
                 
                case "TES002_RC":
                GerarGraficoTes002_RC();
                    break;
                
                case "TES002_CF":
                GerarGraficoTes002_CF();
                    break;
                
                case "TES002_RP":
                GerarGraficoTes002_RP();
                    break;
                
                case "TES002_TS":
                GerarGraficoTes002_TS();
                    break;
                
                case "IMB004":
                GerarGraficoImb004();
                    break;
                
                case "GPS012":
                GerarGraficogps012();
                    break;
                
                case "NOVA014_E001":
                GerarGraficoNOVA014_E001();
                    break;
                
                case "NOVA014_E002":
                GerarGraficoNOVA014_E002();
                    break;
                
                case "NOVA014_E003":
                GerarGraficoNOVA014_E003();
                    break;
                
                case "NOVA014_E004":
                GerarGraficoNOVA014_E004();
                    break;
                case "NOVA014_E005":
                GerarGraficoNOVA014_E005();
                    break;
                case "NOVA014_C001":
                GerarGraficoNOVA014_C001();
                    break;
                case "NOVA014_C002":
                GerarGraficoNOVA014_C002();
                    break;
                case "NOVA014_C003":
                GerarGraficoNOVA014_C003();
                    break;
                case "NOVA014_C004":
                GerarGraficoNOVA014_C004();
                    break;
                case "NOVA014_C005":
                GerarGraficoNOVA014_C005();
                    break;
                case "NOVA014_C006":
                GerarGraficoNOVA014_C006();
                    break;
                case "GR001":
                GerarGraficoGR001();
                    break;
                case "GR003":
                GerarGraficoGR003();
                    break;
                case "GR004":
                GerarGraficoGR004();
                    break;
                case "GR005":
                GerarGraficoGR005();
                    break;
                case "GR006":
                GerarGraficoGR006();
                    break;
                case "GR007":
                GerarGraficoGR007();
                    break;

                default:
                    MessageBox.Show("Nome do Gráfico não reconhecido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
        }

    }
}
