### 📊Projeto Graficos em C#


O presente projeto teve como objetivo criar um programa destinado a apresentar graficos, Pie, Barras, e Linhas com dados de uma base de dados em Oracle num servidor. No programa também é permitido mudar a cor do grafico, exportar o grafico em PDF e em JPG. Os dados na base de dados são dados de obras, oficinas, restaurantes, etc, e surgiu a necessidade de apresentar os dados em um grafico.<br/>

***Funções*** <br/>
Graficos C#-Oracle -> ChartDisplay.cs <br/>

- LoadGraficos: Nem todos o dados podem ser apresentados em graficos de Linhas, Barras ou Pie, alguns graficos exigem receber 3 dados, ou 2, e essa função passa para a variavel "allGraficos" o nome dos graficos que queremos para mais tarde nas funções GerarGrafico escolhermos os graficos que são possiveis apresentar com cada tabela da nossa base de dados. <br/>

- Imagem: A função é chamada pelo botão mudar o tipo de grafico e destina se a obter o index do grafico que esta a ser apresentado quer seja grafico de Linhas, Pie ou barra, por final a função chama a função UpdateGrafico de forma a mudar o tipo de grafico. <br/>

- UpdateGrafico: A função destina se a apresentar o grafico, ou a mudar o tipo de grafico cada vez que é apertado o botão de mudar o tipo de grafico.<br/>

- Funções GerarGrafico: As funções gerar grafico destinam se a primeiramente ir buscar os dados a base de dados e passar os dados para uma lista "listKVP", depois é passado para a lista "activeGraficos", os tipos de graficos que podem ser apresentados com os dados que fomos buscar a base de dados, é feito o mesmo processo para a variavel "activeImages" que vai mostrar e definir o tipo de imagens que o botão de trocar de grafico vai apresentar, e por final é chamado a função "UpdateGrafico" para apresentar o grafico.<br/>

- Escolher_grafico: A função contem um switch e destina se a receber por argumento o nome do grafico da empresa que vamos chamar, quer seja grafico de uma determinada oficina, restaurante, obra, loja, etc., e no final com o swith e chamada a função destinada a apresentar o grafico da determinada empresa. <br/>

- Funções getGrafico: As funções getGrafico estão na classe GraficosDataBase "Graficos C#-Oracle -> GraficosDataBase.cs" e destinam se fazer a conexão com a base de dados e fazer a pesquisa por sqlquery na base de dados no final é retornado uma lista Chave-Valor com os dados obtidos. <br/>

- getFullMonthName: Na base de dados os meses estão de forma numerica, e esta função destina se a converter los para extenso ou seja o nome do mes completo, por exemplo, 1 para Janeiro.<br/>

- getMonthName: Na base de dados os meses estão de forma numerica, e esta função destina se a converter los para extenso mas de forma abreviada, por exemplo, 1 para Jan. <br/>

***Variáveis***
- ConnectionString: A variável destina se a obter os dados da ligação a base de dados. <br/>

- ChartColor: É uma variável do tipo inteiro e destina a conter o index da cor atual do grafico. <br/>

- allGraficos: A lista destina se a conter todos os graficos possiveis "Pie, Barras, Linhas" para posteriormente a lista activeGraficos escolher os graficos possiveis a ser apresentados de acordo os dados na tabela. <br/>

- activeGraficos: A lista destina se a definir os graficos ativos e que podem ser apresentados na lista "allGraficos" de acordo os dados na tabela na base de dados.<br/>

- allImages: Define todas as imagens de graficos "Pie, Barras, Linhas", para posteriormente a variavel "activeImages" poder escolher entre elas quais os graficos podem ser apresentados de acordo os dados na tabela na base de dados. <br/>

- activeImages: A lista destina se a definir as imagens de graficos "Pie, Barras, Linhas" ativas que o botão mudar de grafico vai poder apresentar. <br/>

- CurrentImageIndex: A variável contem o index atual do grafico apresentado, que é util para o botão de mudar de grafico apresentar a imagem do proximo grafico a ser apresentado.<br/>

***Imagens*** <br/>

***Página principal*** <br/>
<p align="center">
  <img src="Pagina principal do programa.png" alt="OpenMontage" width="700">
</p>

***Exemplo grafico de barras*** <br/>
<p align="center">
  <img src="Grafico de barras.png" alt="OpenMontage" width="700">
</p>

***Exemplo grafico de linhas*** <br/>
<p align="center">
  <img src="Grafico de linhas com meses.png" alt="OpenMontage" width="700">
</p>

