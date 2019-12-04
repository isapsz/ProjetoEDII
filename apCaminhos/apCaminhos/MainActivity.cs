using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;
using System.IO;
using Android.Graphics;
using Android.Content;
using Android.Runtime;
using System.Collections.Generic;
using System.Text;


//Amabile Pietrobon Ferreira - 18198
//Isabela Paulino de Souza - 18189
//Gustavo Ferreira Gitzel - 18194

namespace apCaminhos
{
    [Activity(Label = "apCaminho", MainLauncher = true)]
    public class MainActivity : Activity
    {
        const int REQUEST_CIDADE = 1, REQUEST_CAMINHO = 2;

        Button btnMaisCidade, btnMaisCaminho, btnBuscar;
        ImageView imgMapa;
        TextView txtCaminho;
        Grafo grafo;
        bool trocarPagina; //define se estamos indo para uma nova página ou fechando  o app
        BucketHash bucketCidades;
        ListaSimples<Cidade> cidadesNovas;
        ListaSimples<Caminho> caminhosNovos;
        RadioButton rbTempo, rbDistancia;
        Bitmap canvas;
        Spinner spinnerOrigem, spinnerDestino;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            bucketCidades = new BucketHash();
            grafo = new Grafo();
            trocarPagina = false;

            cidadesNovas = new ListaSimples<Cidade>();
            caminhosNovos = new ListaSimples<Caminho>();
            btnMaisCaminho = FindViewById<Button>(Resource.Id.btnCaminho);
            btnMaisCidade = FindViewById<Button>(Resource.Id.btnCidade);
            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            imgMapa = FindViewById<ImageView>(Resource.Id.img);
            rbDistancia = FindViewById<RadioButton>(Resource.Id.rbDistancia);
            rbTempo = FindViewById<RadioButton>(Resource.Id.rbTempo);
            txtCaminho = FindViewById<TextView>(Resource.Id.lvCaminho);
            spinnerOrigem = FindViewById<Spinner>(Resource.Id.spinnerOrigem);
            spinnerDestino = FindViewById<Spinner>(Resource.Id.spinnerDestino);

            //busca o caminho com base nas cidades escolhidas pelo Spinner(comboBox) de origem e destino. Aciona um Toast caso o caminho não exista.
            btnBuscar.Click += (o, e) => {
                
                Cidade origem = bucketCidades[spinnerOrigem.SelectedItem.ToString().ToUpper()];
                Cidade destino = bucketCidades[spinnerDestino.SelectedItem.ToString().ToUpper()];

                if (origem == null || destino == null || origem.Equals(destino))
                    Toast.MakeText(ApplicationContext, "Erro. Preencha todos os campos corretamente...", ToastLength.Long).Show();
                else
                    DesenharCaminho(origem.Id, destino.Id, (rbDistancia.Checked) ? Grafo.Pesos.distancia : Grafo.Pesos.tempo);
                
            };

            //Abre uma nova página para adicionar o caminho.
            btnMaisCaminho.Click += (o, e) =>{
                trocarPagina = true;
                IncluirNoArquivo(REQUEST_CAMINHO);
            };

            //Abre uma nova página para adicionar a cidade. 
            btnMaisCidade.Click += (o, e) => {
                trocarPagina = true;
                IncluirNoArquivo(REQUEST_CIDADE);
            };

            canvas = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            imgMapa.SetImageBitmap(canvas);
            LerArquivos();
        }

        //chama o método de definir o caminho pelo grafo, usando Dijkstra e depois o exibe.
        private void DesenharCaminho(int origem, int destino, Grafo.Pesos peso)
        {
            DistOriginal[] caminho = grafo.Caminho(origem, destino, peso);
            txtCaminho.Text = ExibirPercurso(origem, destino, caminho); //exibe o percurso e desenha no mapa
        }

        //Exibe o percurso no mapa e retorna uma string com todas as cidades percorridas e o tempo e a distância do percurso.
        private string ExibirPercurso(int inicioDoPercurso, int finalDoPercurso, DistOriginal[] caminho)
        {
            List<string> percurso = new List<string>();
            Paint p = new Paint();
            p.StrokeWidth = 15;
            Bitmap btm = canvas.Copy(canvas.GetConfig(), true);
            Canvas c = new Canvas(btm);
            string resultado = "";
            resultado += "Caminho:\n\n";
            bool temCaminho = true;

            p.Color = Color.Red;
            int onde = finalDoPercurso, anterior = 0, tempo = 0, distancia = 0;
            Stack<string> pilha = new Stack<string>();

            c.DrawCircle(bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaX * btm.Width,
                         bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaY * btm.Height,
                         20, p);

            int aux = 0;
            while (onde != inicioDoPercurso)
            {
                anterior = onde;
                onde = caminho[onde].verticePai;

                c.DrawCircle(bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaX * btm.Width,
                             bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaY * btm.Height,
                             20, p);

                c.DrawLine(bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaX * btm.Width,
                           bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaY * btm.Height, 
                           bucketCidades[grafo[anterior].rotulo.ToUpper()].CoordenadaX * btm.Width,
                           bucketCidades[grafo[anterior].rotulo.ToUpper()].CoordenadaY * btm.Height,
                           p);

                tempo += grafo[onde, anterior].Tempo;
                distancia += grafo[onde, anterior].Distancia;

                pilha.Push(grafo[onde].rotulo);
                aux++;
            }

            while (pilha.Count != 0)
            {
                resultado += pilha.Pop() + "\n";
            }

            if (aux == 1 && caminho[finalDoPercurso].peso == Grafo.infinity)
                temCaminho = false;
            
            else
                resultado += grafo[finalDoPercurso].rotulo + "\n";

            resultado += "Tempo: " + tempo + "min\nDistancia: " + distancia + "km";

            if (!temCaminho)
            {
                Toast.MakeText(ApplicationContext, "Não existem caminhos entre essas duas cidades", ToastLength.Long).Show();
                return "";
            }

            imgMapa.SetImageBitmap(btm);
            return resultado;
        }

        //Chama as Activitys(páginas) para adicionar Caminho e Cidade no arquivo com base no parâmetro passado.
        private void IncluirNoArquivo(int request)
        {
            Intent intent;
            if (request == REQUEST_CIDADE)
                intent = new Intent(this, typeof(InclusaoView));
            else
                intent = new Intent(this, typeof(InclusaoCaminhoActivity));
            StartActivityForResult(intent, request);
        }

        //Recebe os valores de cidade ou caminho e trata inserindo em listas de novos valores.
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            switch(requestCode)
            {
                case REQUEST_CIDADE:
                    if(resultCode == Result.Ok)
                    {
                        Cidade nova = new Cidade(grafo.NumVerts, data.GetStringExtra("nome"), data.GetFloatExtra("x", 0f), data.GetFloatExtra("y", 0f));
                        grafo.NovoVertice(nova.Nome);
                        cidadesNovas.InserirAposFim(nova);
                        nova.Nome = nova.Nome.ToUpper();
                        bucketCidades.Insert(nova);
                    }
                    break;

                case REQUEST_CAMINHO:
                    if (resultCode == Result.Ok)
                    {
                       
                        Caminho novoCaminho = new Caminho(data.GetStringExtra("origem"),
                                                          data.GetStringExtra("destino"),
                                                          data.GetIntExtra("distancia", 0),
                                                          data.GetIntExtra("tempo", 0));

                        Cidade origem = bucketCidades[novoCaminho.Origem.ToUpper()];
                        Cidade destino = bucketCidades[novoCaminho.Destino.ToUpper()];

                        if(origem == null || destino == null)
                            Toast.MakeText(ApplicationContext, "Erro. Preencha com Cidades válidas", ToastLength.Long).Show();
                        else if(grafo[origem.Id, destino.Id].Distancia == Grafo.infinity)
                        {
                            caminhosNovos.InserirAposFim(novoCaminho);

                            grafo.NovaAresta(origem.Id,
                                             destino.Id,
                                             novoCaminho.Distancia,
                                             novoCaminho.Tempo);
                            grafo.NovaAresta(destino.Id,
                                             origem.Id,
                                             novoCaminho.Distancia,
                                             novoCaminho.Tempo);
                        }
                    }
                    break; 
            }

            trocarPagina = false;
        }

        //Lê os arquivos de cidades e caminhos armazenados, em primeiro momento no Assets e depois na memória interna do celular.
        private void LerArquivos()
        {
            string sandbox = FilesDir.AbsolutePath;

            string caminhoArquivo = System.IO.Path.Combine(sandbox, "Cidades.txt");
            List<string> cidades = new List<string>();
            if (!File.Exists(caminhoArquivo))
            {
                using (StreamReader arquivoAsset = new StreamReader(Assets.Open("Cidades.txt")))
                {
                    StreamWriter saidaArquivoInterno = new StreamWriter(caminhoArquivo);
                    while (!arquivoAsset.EndOfStream)
                    {
                        Cidade cidade = Cidade.LerRegistro(arquivoAsset);
                        cidades.Add(cidade.Nome);
                        bucketCidades.Insert(cidade);
                        grafo.NovoVertice(cidade.Nome);
                        saidaArquivoInterno.WriteLine(cidade.ParaArquivo());
                    }
                    saidaArquivoInterno.Close();
                }
            }
            else
            {
                using (StreamReader arquivoInterno = new StreamReader(caminhoArquivo))
                {
                    while (!arquivoInterno.EndOfStream)
                    {
                        Cidade cidade = Cidade.LerRegistro(arquivoInterno);
                        bucketCidades.Insert(cidade);
                        cidades.Add(cidade.Nome);
                        grafo.NovoVertice(cidade.Nome);
                    }

                }
            }

            spinnerOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, cidades);
            spinnerDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, cidades);
            caminhoArquivo = System.IO.Path.Combine(sandbox, "GrafoTremEspanhaPortugal.txt");


            if (!File.Exists(caminhoArquivo))
            {
                using (StreamReader arquivoAsset = new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt")))
                {
                    StreamWriter saidaArquivoInterno = new StreamWriter(caminhoArquivo);
                    while (!arquivoAsset.EndOfStream)
                    {
                        Caminho caminho = Caminho.LerRegistro(arquivoAsset);
                        grafo.NovaAresta(bucketCidades[caminho.Origem].Id, bucketCidades[caminho.Destino].Id, caminho.Distancia, caminho.Tempo);
                        saidaArquivoInterno.WriteLine(caminho.ParaArquivo());
                    }
                    
                    saidaArquivoInterno.Close();
                }
            }
            else
            {
                using (StreamReader arquivoInterno = new StreamReader(caminhoArquivo))
                {
                    while (!arquivoInterno.EndOfStream)
                    {
                        Caminho caminho = Caminho.LerRegistro(arquivoInterno);
                        grafo.NovaAresta(bucketCidades[caminho.Origem].Id, bucketCidades[caminho.Destino].Id, caminho.Distancia, caminho.Tempo);
                        grafo.NovaAresta(bucketCidades[caminho.Destino].Id, bucketCidades[caminho.Origem].Id, caminho.Distancia, caminho.Tempo);
                    }
                }
            }
        }

        //Chama o método para escrever os novos valores no arquivo texto que fica na memória interna do celular caso estejamos fechando o app,
        //ou seja, quando a booleana "trocarPagina" estiver falsa.
        protected override void OnStop(){
            base.OnStop();

            if (!trocarPagina) 
                EscreverArquivos();
        }

        //Método para escrever nos arquivos internos do celular por meio das listas que guardam as novas cidades e caminhos incluidos.
        private void EscreverArquivos()
        {
            string sandbox = FilesDir.AbsolutePath;
            
            cidadesNovas.ParaArquivo(new StreamWriter(System.IO.Path.Combine(sandbox, "Cidades.txt"), true));
            
            caminhosNovos.ParaArquivo(new StreamWriter(System.IO.Path.Combine(sandbox, "GrafoTremEspanhaPortugal.txt"), true));
        }
    }
}

