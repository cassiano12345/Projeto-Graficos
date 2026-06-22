using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Globalization;
using System.Windows;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;


namespace Graficos
{
    class GraficosDataBase
    {
        public static String ConnectionString = Properties.Settings.Default.ConStr; //Obtendo os dados de ligação a base de dados
        public static String ConnectionString_ = Properties.Settings.Default.ConStr_; //Obtendo os dados de ligação a base de dados Mysql

        
        public static List<KeyValuePair<String, Double>> getGraficoOFI008(String codemp, String codutil, int ano)
        {
            List<KeyValuePair<String, Double>> kvpList = new List<KeyValuePair<string, Double>>();
            String sqlquery = "select Valor01, Valor02, Valor03, Valor04, Valor05, Valor06, Valor07, Valor08, Valor09, Valor10, Valor11, Valor12 " +
                    " from Temp_OFI_Vendas " +
                        " where codemp = " + codemp +
                    " and ano = " + ano +
                    " and codutil =  " + codutil;
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {

                        kvpList = new List<KeyValuePair<string, Double>>{
                            new KeyValuePair<String, Double>("JAN", Double.Parse(dr[0].ToString())),
                            new KeyValuePair<String, Double>("FEV", Double.Parse(dr[1].ToString())),
                            new KeyValuePair<String, Double>("MAR", Double.Parse(dr[2].ToString())),
                            new KeyValuePair<String, Double>("ABR", Double.Parse(dr[3].ToString())),
                            new KeyValuePair<String, Double>("MAI", Double.Parse(dr[4].ToString())),
                            new KeyValuePair<String, Double>("JUN", Double.Parse(dr[5].ToString())),
                            new KeyValuePair<String, Double>("JUL", Double.Parse(dr[6].ToString())),
                            new KeyValuePair<String, Double>("AGO", Double.Parse(dr[7].ToString())),
                            new KeyValuePair<String, Double>("SET", Double.Parse(dr[8].ToString())),
                            new KeyValuePair<String, Double>("OUT", Double.Parse(dr[9].ToString())),
                            new KeyValuePair<String, Double>("NOV", Double.Parse(dr[10].ToString())),
                            new KeyValuePair<String, Double>("DEZ", Double.Parse(dr[11].ToString()))};
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            return kvpList;
        }

        public static List<List<KeyValuePair<string, Double>>> getGraficoNOVA015_A(string codemp, string codutil, int ano)
        {
            List<List<KeyValuePair<string, Double>>> finalList = new List<List<KeyValuePair<string, Double>>>();
            List<KeyValuePair<string, Double>> kvpListV1 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<string, Double>> kvpListV2 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<string, Double>> kvpListV3 = new List<KeyValuePair<string, Double>>();

            string sqlquery = "Select CAST(Valor1 as FLOAT) V1, Valor2, Valor3, Valor4, Valor5 From Temp_Graf Where CodEmp=" + codemp + " and CodUtil=" + codutil + " and Graf='cli_fact' Order by V1 asc";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();

                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");

                        KeyValuePair<string, Double> tempKvpV1 = new KeyValuePair<string, Double>(getMonthName(dr[0].ToString().Trim()), Double.Parse(dr[1].ToString(), culture));
                        KeyValuePair<string, Double> tempKvpV2 = new KeyValuePair<string, Double>(getMonthName(dr[0].ToString().Trim()), Double.Parse(dr[2].ToString(), culture));
                        KeyValuePair<string, Double> tempKvpV3 = new KeyValuePair<string, Double>(getMonthName(dr[0].ToString().Trim()), Double.Parse(dr[3].ToString(), culture));

                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);

            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA015_BM(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, Double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, Double>>();
            String sqlquery = "Select NOME,nvl(VALOR2,0),nvl(QTD,0),nvl(QTD2,0) From TEMP_FTTOP Where CodEmp=" + codemp + " and CodUtil=" + codutil + " Order by CLIENTE asc";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[1].ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[2].ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[3].ToString(), culture));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);
            return finalList;
        }


        /*
        public static List<KeyValuePair<String, Double>> getGraficoOBR002(String codemp, String codutil)
        {
            List<KeyValuePair<String, Double>> kvpList = new List<KeyValuePair<string, Double>>();

            String sqlquery = "select VALOR01, VALOR02 from temp_obr where codemp = " + codemp + " and codutil = " + codutil + " ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {

                        kvpList = new List<KeyValuePair<string, Double>>{
                            new KeyValuePair<String, Double>("Recebido", Double.Parse(dr[0].ToString())),
                            new KeyValuePair<String, Double>("A Receber", Double.Parse(dr[1].ToString()))};
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            return kvpList;

        }
        */

        public static List<List<KeyValuePair<String, Double>>> getGraficoOBR013_AC(String codemp, String codutil, String opcao)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV4 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV5 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV6 = new List<KeyValuePair<string, double>>();


            String sqlquery = "Select MES, ANO, Valor01,Valor02,Valor03,Valor04,Valor05,Valor06 From Temp_Obr Where CodEmp=" + codemp + " and codutil=" + codutil + " and opcao='" + opcao + "' Order by ANO ASC, MES asc ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(2) ? 0.0 : Double.Parse(dr.GetValue(2).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(3) ? 0.0 : Double.Parse(dr.GetValue(3).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(4) ? 0.0 : Double.Parse(dr.GetValue(4).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV4 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(5) ? 0.0 : Double.Parse(dr.GetValue(5).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV5 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(6) ? 0.0 : Double.Parse(dr.GetValue(6).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV6 = new KeyValuePair<string, double>(getMonthName(dr.IsDBNull(0) ? "" : dr.GetValue(0).ToString().Trim()).Substring(0, 3) + "/" + dr.GetValue(1).ToString(), dr.IsDBNull(7) ? 0.0 : Double.Parse(dr.GetValue(7).ToString(), culture));

                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                        kvpListV4.Add(tempKvpV4);
                        kvpListV5.Add(tempKvpV5);
                        kvpListV6.Add(tempKvpV6);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);
            finalList.Add(kvpListV4);
            finalList.Add(kvpListV5);
            finalList.Add(kvpListV6);

            return finalList;
        }

        public static List<KeyValuePair<String, Double>> getGraficoOBR013_FT(String codemp, String codutil, String opcao)
        {
            List<KeyValuePair<String, Double>> kvpList = new List<KeyValuePair<string, double>>();


            String sqlquery = "select SUM(VALOR01), SUM(VALOR02) from temp_obr where codemp = " + codemp + " and codutil = " + codutil + " and opcao = '" + opcao + "' ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    //kvpList = new List<KeyValuePair<string, double>>{};
                    while (dr.Read())
                    {
                        Console.WriteLine("V1: " + Double.Parse(dr[0].ToString())+" V2: "+ Double.Parse(dr[1].ToString()));
                        kvpList.Add(new KeyValuePair<String, Double>("Recebido", Double.Parse(dr[0].ToString())));
                        kvpList.Add(new KeyValuePair<String, Double>("A Receber", Double.Parse(dr[1].ToString())));
                        // new KeyValuePair<String, Double>("A Receber", Double.Parse(dr[1].ToString()))
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            return kvpList;

        }

     


        public static List<List<KeyValuePair<String, Double>>> getGraficoOBR013_CT(String codemp, String codutil, String opcao)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV4 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV5 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV6 = new List<KeyValuePair<string, double>>();

            String sqlquery = "Select SUM(Valor01),SUM(Valor02),SUM(Valor03),SUM(Valor04),SUM(Valor05) From Temp_Obr Where CodEmp=" + codemp + " and codutil=" + codutil + " and opcao='" + opcao + "'";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>("Equipamentos", dr.IsDBNull(0) ? 0.0 : Double.Parse(dr.GetValue(0).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>("Mão Obra", dr.IsDBNull(1) ? 0.0 : Double.Parse(dr.GetValue(1).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>("T. Custos", dr.IsDBNull(2) ? 0.0 : Double.Parse(dr.GetValue(2).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV4 = new KeyValuePair<string, double>("Sub-Emp", dr.IsDBNull(3) ? 0.0 : Double.Parse(dr.GetValue(3).ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV5 = new KeyValuePair<string, double>("Materiais", dr.IsDBNull(4) ? 0.0 : Double.Parse(dr.GetValue(4).ToString(), culture));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV1.Add(tempKvpV2);
                        kvpListV1.Add(tempKvpV3);
                        kvpListV1.Add(tempKvpV4);
                        kvpListV1.Add(tempKvpV5); 
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            listaFinal.Add(kvpListV1);
            List<KeyValuePair<String, Double>> listaNaPosicaoZero = listaFinal[0];
            List<List<KeyValuePair<String, Double>>> listaDeListas = new List<List<KeyValuePair<string, double>>>();
            kvpListV2.Add(listaNaPosicaoZero[0]);
            kvpListV3.Add(listaNaPosicaoZero[1]);
            kvpListV4.Add(listaNaPosicaoZero[2]);
            kvpListV5.Add(listaNaPosicaoZero[3]);
            kvpListV6.Add(listaNaPosicaoZero[4]);
            listaFinal.Clear();
            listaFinal.Add(kvpListV2);
            listaFinal.Add(kvpListV3);
            listaFinal.Add(kvpListV4);
            listaFinal.Add(kvpListV5);
            listaFinal.Add(kvpListV6);
            return listaFinal;
        }




        public static List<KeyValuePair<String, Double>> getGraficoCRM008_FT(String codemp)
        {
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select substr(b.NRVD||'-'||b.nome,0,12) nome,a.prev_m1 Real,a.prev_m2 Prev,a.prev_m3 Desv " +
                        " from temp_cmr_custos a, ctb_vendedor b " +
                        " where a.codemp=" + codemp + " and a.codigo='FACT' and " +
                        "           b.codemp=a.codemp and " +
                        "           b.nrvd=a.nrvd " +
                        " order by 1";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(3).ToString()));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            return kvpListV1;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoCRM008_VS(String codemp)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select substr(b.nome,0,10) nome,a.prev_m1 VISITAS,a.prev_m2 VENDAS " +
                        " from temp_cmr_custos a, ctb_vendedor b " +
                        " where a.codemp=" + codemp + " and a.codigo='VISITA' and " +
                        "           b.codemp=a.codemp and " +
                        "           b.nrvd=a.nrvd " +
                        " order by 1 ";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;
        }
        public static List<List<KeyValuePair<String, Double>>> getGraficoCRM008_RS1(String codemp)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select codemp, ano, Prev_M1 Vendas, prev_M2 Despesas,prev_M3 Margem " +
                            " from temp_cmr_custos " +
                            " where codemp=" + codemp + " and codigo = 'RESUMO' " +
                            " order by Ano";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[1].ToString(), double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[1].ToString(), double.Parse(dr.GetValue(3).ToString()));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>(dr[1].ToString(), double.Parse(dr.GetDouble(4).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);

            return finalList;
        }

        public static List<KeyValuePair<String, Double>> getGraficoCRM008_RS2(String codemp)
        {
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select codemp ,Prev_M1 Vendas,prev_M2 Despesas " +
                                " from temp_cmr_custos " +
                                " where codemp=" + codemp + " and codigo = 'VENDAS'";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>("Vendas", dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>("Despesas", dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV1.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            return kvpListV1;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoMAN006_C(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select MES, VALOR1, VALOR2 " +
                            " from temp_man " +
                            " where codemp = " + codemp +
                            "  and codutil = " + codutil +
                            "  and opcao = 'CM'";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(getMonthName(dr.GetValue(0).ToString()), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(getMonthName(dr.GetValue(0).ToString()), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);

            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoMAN006_T(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select MES, VALOR3 " +
                            " from temp_man " +
                            " where codemp = " + codemp +
                            "  and codutil = " + codutil +
                            "  and opcao = 'CM'";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(getMonthName(dr.GetValue(0).ToString()), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_DC(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1, VALOR2 " +
                                " from temp_tes " +
                                " where codemp  = " + codemp +
                                " and codutil   = '" + codutil + "' " +
                                " and opcao     = 'DC'" +
                                " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_DB(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1, VALOR2 " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'DB'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_GV(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1, VALOR2 " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'GV'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_GC(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1, VALOR2 " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'GC'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }
        public static List<KeyValuePair<String, Double>> getGraficoTes002_LU(String codemp, String codutil)
        {
            List<KeyValuePair<String, Double>> finalList = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1  " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'LU'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            return kvpListV1;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_CU(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select CONTA, VALOR1, VALOR2, VALOR3  " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'CU'" +
                                 " order by CONTA asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(2));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(3));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_PR(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select CONTA, VALOR1, VALOR2, VALOR3   " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'PR'" +
                                 " order by CONTA asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);

            return finalList;
        }

        public static List<KeyValuePair<String, Double>> getGraficoTes002_RC(String codemp, String codutil, String conta)
        {
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1 " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'RC'" +
                                 " and conta     = '" + conta + "'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            return kvpListV1;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_CF(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = " select to_char(data,'MM') MES, VALOR1, VALOR2 " +
                                 " from temp_tes " +
                                 " where codemp  = " + codemp +
                                 " and codutil   = '" + codutil + "' " +
                                 " and opcao     = 'CF'" +
                                 " order by 1 asc ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((dr.GetString(0).ToString().ElementAt(0).ToString().Equals("0") ? getMonthName(dr.GetValue(0).ToString().Substring(1, 1)) : getMonthName(dr.GetValue(0).ToString())), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);

            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_RP(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select DATA, valor1, valor2 " +
                                " from temp_tes " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' " +
                                " and opcao = 'RP'" +
                            " order by 1 asc";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetDateTime(0).ToShortDateString(), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetDateTime(0).ToShortDateString(), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoTes002_TS(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select DATA, valor1, valor2, valor3 " +
                                " from temp_tes " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' " +
                                " and opcao = 'TS'" +
                            " order by 1 asc";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetDateTime(0).ToShortDateString(), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetDateTime(0).ToShortDateString(), dr.GetDouble(2));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>(dr.GetDateTime(0).ToShortDateString(), dr.GetDouble(3));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);
            listaFinal.Add(kvpListV3);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoImb004(String codemp, String codutil, int ano)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV4 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV5 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV6 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select  ANO, VALOR01, VALOR02, VALOR03, VALOR04, VALOR05, VALOR06 " +
                                " from TEMP_IMB_GRAFICO " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' " +
                                " and ano = '" + ano + "' " +
                            " order by 1 asc";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>((ano).ToString(), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>((ano - 1).ToString(), dr.GetDouble(2));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, double>((ano - 2).ToString(), dr.GetDouble(3));
                        KeyValuePair<String, Double> tempKvpV4 = new KeyValuePair<string, double>((ano - 3).ToString(), dr.GetDouble(4));
                        KeyValuePair<String, Double> tempKvpV5 = new KeyValuePair<string, double>((ano - 4).ToString(), dr.GetDouble(5));
                        KeyValuePair<String, Double> tempKvpV6 = new KeyValuePair<string, double>((ano - 5).ToString(), dr.GetDouble(6));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                        kvpListV4.Add(tempKvpV4);
                        kvpListV5.Add(tempKvpV5);
                        kvpListV6.Add(tempKvpV6);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);
            listaFinal.Add(kvpListV3);
            listaFinal.Add(kvpListV4);
            listaFinal.Add(kvpListV5);
            listaFinal.Add(kvpListV6);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_E001(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 ";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_E002(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY QUEBRA01 ";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_E003(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select QUEBRA02, NVL(SUM(VALOR),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY QUEBRA02 ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        //KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetString(1), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            
            listaFinal.Add(kvpListV1);

            return listaFinal;
        }


        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_E004(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select QUEBRA02, NVL(SUM(VALOR),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY QUEBRA02 ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_E005(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 " +
                " ORDER BY LPAD(QUEBRA01,'0',2)";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C001(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 " +
                "  ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;

        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C002(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY QUEBRA01 " +
                " ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;

        }
        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C003(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 " +
                "ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;

        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C004(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY QUEBRA01 " +
                " ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C005(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 " +
                " ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;

        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA014_C006(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();

            String sqlquery = "select QUEBRA01, NVL(SUM(VALOR),0), NVL(SUM(VALORANT),0) " +
                " from TEMP_FTTOP " +
                " where CodEmp=" + codemp + " and CodUtil= " + codutil +
                " GROUP BY CODEMP,CODUTIL, QUEBRA01 " +
                " ORDER BY LPAD(QUEBRA01,2, '0')";


            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(2).ToString()));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[0].ToString(), Double.Parse(dr.GetValue(1).ToString()));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);


            return finalList;

        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR001(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select CODIGO, V1 " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' AND V1 IS NOT NULL ORDER BY CODIGO ASC";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR003(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select CODIGO, V1 " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' AND V1 IS NOT NULL ORDER BY CODIGO ASC";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR004(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select CODIGO, V1 " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "'  AND V1 IS NOT NULL ORDER BY CODIGO ASC";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR005(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select CODIGO, SUM(V1)V1, SUM(V2) V2  " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' GROUP BY CODIGO ORDER BY CODIGO ASC";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), (dr.IsDBNull(1) ? 0 : dr.GetDouble(1)));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetString(0), (dr.IsDBNull(2) ? 0 : dr.GetDouble(2)));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR006(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select CODIGO, V1, V2 " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "' AND V1 IS NOT NULL order by codigo asc";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(2));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            listaFinal.Add(kvpListV2);
            return listaFinal;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoGR007(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> listaFinal = new List<List<KeyValuePair<string, double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, double>>();
            String sqlquery = "select NOME, SUM(V1)V1 " +
                                " from TEMP_FT " +
                            " where codemp = " + codemp +
                                " and codutil = '" + codutil + "'AND NOME IS NOT NULL GROUP BY NOME";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr.GetString(0), dr.GetDouble(1));
                        kvpListV1.Add(tempKvpV1);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            listaFinal.Add(kvpListV1);
            return listaFinal;
        }
        public static List<List<KeyValuePair<String, Double>>> getGraficoNOVA015_BA(String codemp, String codutil)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, Double>>>();
            List<KeyValuePair<String, Double>> kvpListV1 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<String, Double>> kvpListV2 = new List<KeyValuePair<string, Double>>();
            List<KeyValuePair<String, Double>> kvpListV3 = new List<KeyValuePair<string, Double>>();
            String sqlquery = "Select NOME,nvl(VALOR2,0),nvl(QTD,0),nvl(QTD2,0) From TEMP_FTTOP Where CodEmp=" + codemp + " and CodUtil=" + codutil + " Order by CLIENTE asc";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                        KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[1].ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[2].ToString(), culture));
                        KeyValuePair<String, Double> tempKvpV3 = new KeyValuePair<string, Double>(dr[0].ToString().Trim(), Double.Parse(dr[3].ToString(), culture));
                        kvpListV1.Add(tempKvpV1);
                        kvpListV2.Add(tempKvpV2);
                        kvpListV3.Add(tempKvpV3);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            finalList.Add(kvpListV1);
            finalList.Add(kvpListV2);
            finalList.Add(kvpListV3);
            return finalList;
        }

        public static List<List<KeyValuePair<String, Double>>> getGraficoCRM008_CT(String codemp, String nrvd, String trimestre)
        {
            List<List<KeyValuePair<String, Double>>> finalList = new List<List<KeyValuePair<string, double>>>();

            List<KeyValuePair<String, Double>> alimReal = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> alimPrev = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> CombReal = new List<KeyValuePair<string, double>>();
            List<KeyValuePair<String, Double>> CombPrev = new List<KeyValuePair<string, double>>();

            String sqlquery = "select a.codemp,a.nrvd,a.ano,substr(b.descricao,0,5)||'.', a.Prev_M1 Prev,a.Real_M1 Real, " +
                    " decode(nvl(" + trimestre + ",1),1,'01 - Jan', " +
                    "                             2,'04 - Abr'," +
                    "                     3,'07 - Jul', " +
                    "                     4,'10 - Out') Mes " +
                    " from temp_cmr_custos a,cmr_tabelas b " +
                    " where a.codemp=" + codemp + " and a.nrvd=" + nrvd + " and a.codigo != 'FACT' and " +
                    "      b.codemp=a.codemp and " +
                    "      b.tabela='CT' and " +
                    "      b.codigo=a.codigo " +
                    " union " +
                    " select a.codemp,a.nrvd,a.ano,substr(b.descricao,0,5)||'.', a.Prev_M2 Prev,a.Real_M2 Real, " +
                    " decode(nvl(" + trimestre + ",1),1,'02 - Fev', 2,'05 - Mai', 3,'08 - Ago', 4,'11 - Nov') Mes " +
                    " from temp_cmr_custos a,cmr_tabelas b " +
                    " where a.codemp=" + codemp + " and a.nrvd=" + nrvd + " and " +
                     " a.codigo != 'FACT' and " +
                         " b.codemp=a.codemp and " +
                         " b.tabela='CT' and " +
                         " b.codigo=a.codigo " +
                    "union " +
                    "select a.codemp,a.nrvd,a.ano,substr(b.descricao,0,5)||'.', a.Prev_M3 Prev,a.Real_M3 Real, " +
                    "decode(nvl(" + trimestre + ",1),1,'03 - Mar', 2,'06 - Jun', 3,'09 - Set',4,'12 - Dez') Mes " +
                    " from temp_cmr_custos a,cmr_tabelas b " +
                    " where a.codemp=" + codemp + " and a.nrvd=" + nrvd + " and " +
                    " a.codigo != 'FACT' and " +
                         " b.codemp=a.codemp and " +
                         " b.tabela='CT' and " +
                         " b.codigo=a.codigo " +
                    " order by 7,4 ";
            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    int count = 0;
                    while (dr.Read())
                    {
                        if (count == 0)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            alimPrev.Add(tempKvpV1);
                            alimReal.Add(tempKvpV2);
                        }
                        else if (count == 1)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            CombPrev.Add(tempKvpV1);
                            CombReal.Add(tempKvpV2);
                        }
                        else if (count == 2)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            alimPrev.Add(tempKvpV1);
                            alimReal.Add(tempKvpV2);
                        }
                        else if (count == 3)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            CombPrev.Add(tempKvpV1);
                            CombReal.Add(tempKvpV2);
                        }
                        else if (count == 4)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            alimPrev.Add(tempKvpV1);
                            alimReal.Add(tempKvpV2);
                        }
                        else if (count == 5)
                        {
                            KeyValuePair<String, Double> tempKvpV1 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(4).ToString()));
                            KeyValuePair<String, Double> tempKvpV2 = new KeyValuePair<string, double>(dr[6].ToString(), Double.Parse(dr.GetValue(5).ToString()));
                            CombPrev.Add(tempKvpV1);
                            CombReal.Add(tempKvpV2);
                        }
                        count++;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }

            finalList.Add(alimPrev);
            finalList.Add(alimReal);
            finalList.Add(CombPrev);
            finalList.Add(CombReal);

            return finalList;
        }

        

        // Grafico Mysql

        public static List<KeyValuePair<string, double>> getGraficoOBR002(string codemp, string codutil)
        {
            List<KeyValuePair<string, double>> kvpList = new List<KeyValuePair<string, double>>();

            string sqlquery = "SELECT VALOR01, VALOR02 FROM temp_obr WHERE codemp = '"+ codemp +"' AND codutil = '"+ codutil +"'";

            using (MySqlConnection conn = new MySqlConnection(ConnectionString_))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand(sqlquery, conn))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            kvpList = new List<KeyValuePair<string, double>>
                    {
                        new KeyValuePair<string, double>(
                            "Recebido",
                            Convert.ToDouble(dr["VALOR01"])
                        ),
                        new KeyValuePair<string, double>(
                            "A Receber",
                            Convert.ToDouble(dr["VALOR02"])
                        )
                    };
                        }
                    }
                }
            }

            return kvpList;
        }



        #region ----  Grafico com Bug  ----
        public static List<KeyValuePair<String, Double>> getGraficogps012(String codemp, String codutil, int ano, String opcao)
        {
            List<KeyValuePair<String, Double>> kvpList = new List<KeyValuePair<string, double>>();
            //converter string anos

            String sqlquery = "select v1,v2,v3,q1,q2 from temp_ft where codemp = " + codemp + " and codutil = " + codutil + " and opcao = '" + opcao + "' ";

            using (OracleConnection conn = new OracleConnection(ConnectionString))
            {
                OracleCommand cmd = new OracleCommand(sqlquery, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                try
                {
                    while (dr.Read())
                    {
                        CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                        kvpList = new List<KeyValuePair<string, double>>{
                            new KeyValuePair<String, Double>(ano.ToString(), Double.Parse(dr[0].ToString())),
                            new KeyValuePair<String, Double>((ano-1).ToString(), Double.Parse(dr[1].ToString(), culture)),
                            new KeyValuePair<String, Double>((ano-2).ToString(), Double.Parse(dr[2].ToString(), culture)),
                            new KeyValuePair<String, Double>((ano-3).ToString(), Double.Parse(dr[3].ToString(), culture)),
                            new KeyValuePair<String, Double>((ano-4).ToString(), Double.Parse(dr[4].ToString(), culture))};
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                }
                finally
                {
                    dr.Close();
                }
            }
            return kvpList;

        }




        #endregion

        public static String getFullMonthName(String idMonth)
        {
            if (idMonth.Equals("1"))
            {
                return "Janeiro";
            }
            else if (idMonth.Equals("2"))
            {
                return "Fevereiro";
            }
            else if (idMonth.Equals("3"))
            {
                return "Março";
            }
            else if (idMonth.Equals("4"))
            {
                return "Abril";
            }
            else if (idMonth.Equals("5"))
            {
                return "Maio";
            }
            else if (idMonth.Equals("6"))
            {
                return "Junho";
            }
            else if (idMonth.Equals("7"))
            {
                return "Julho";
            }
            else if (idMonth.Equals("8"))
            {
                return "Agosto";
            }
            else if (idMonth.Equals("9"))
            {
                return "Setembro";
            }
            else if (idMonth.Equals("10"))
            {
                return "Outubro";
            }
            else if (idMonth.Equals("11"))
            {
                return "Novembro";
            }
            else if (idMonth.Equals("12"))
            {
                return "Dezembro";
            }
            return "";
        }

        public static String getMonthName(String idMonth)
        {
            if (idMonth.Equals("1"))
            {
                return "Jan";
            }
            else if (idMonth.Equals("2"))
            {
                return "Fev";
            }
            else if (idMonth.Equals("3"))
            {
                return "Mar";
            }
            else if (idMonth.Equals("4"))
            {
                return "Abr";
            }
            else if (idMonth.Equals("5"))
            {
                return "Mai";
            }
            else if (idMonth.Equals("6"))
            {
                return "Jun";
            }
            else if (idMonth.Equals("7"))
            {
                return "Jul";
            }
            else if (idMonth.Equals("8"))
            {
                return "Ago";
            }
            else if (idMonth.Equals("9"))
            {
                return "Set";
            }
            else if (idMonth.Equals("10"))
            {
                return "Out";
            }
            else if (idMonth.Equals("11"))
            {
                return "Nov";
            }
            else if (idMonth.Equals("12"))
            {
                return "Dez";
            }

            return "";
        }
    }
}
