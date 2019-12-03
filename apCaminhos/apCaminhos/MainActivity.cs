using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Runtime;
using System.Collections.Generic;
using Java.IO;

namespace apCaminhos
{
    [Activity(Label = "apCaminho", MainLauncher = true)]
    public class MainActivity : Activity
    {
        const int REQUEST_CIDADE = 1, REQUEST_CAMINHO = 2;

        Button btnMaisCidade, btnMaisCaminho, btnBuscar;
        ImageView imgMapa;
        EditText edtOrigem, edtDestino;
        TextView txtCaminho;
        Grafo grafo;
        bool trocarPagina;
        BucketHash bucketCidades;
        ListaSimples<Cidade> cidadesNovas;
        ListaSimples<Caminho> caminhosNovos;
        RadioButton rbTempo, rbDistancia;
        Bitmap canvas;

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
            edtDestino = FindViewById<EditText>(Resource.Id.edtDestino);
            edtOrigem = FindViewById<EditText>(Resource.Id.edtOrigem);
            rbDistancia = FindViewById<RadioButton>(Resource.Id.rbDistancia);
            rbTempo = FindViewById<RadioButton>(Resource.Id.rbTempo);
            txtCaminho = FindViewById<TextView>(Resource.Id.txtCaminho);
            
            btnBuscar.Click += (o, e) => {
                if (string.IsNullOrWhiteSpace(edtDestino.Text)  || string.IsNullOrWhiteSpace(edtOrigem.Text))
                    Toast.MakeText(ApplicationContext, "Preencha o nome das cidades.", ToastLength.Long).Show();
                else
                {
                    Cidade origem = bucketCidades[edtOrigem.Text.ToUpper()];
                    Cidade destino = bucketCidades[edtDestino.Text.ToUpper()];

                    if (origem == null || destino == null || origem.Equals(destino))
                        Toast.MakeText(ApplicationContext, "Erro. Preencha todos os campos corretamente...", ToastLength.Long).Show();
                    else
                     DesenharCaminho(origem.Id, destino.Id, (rbDistancia.Checked) ? Grafo.Pesos.distancia : Grafo.Pesos.tempo);
                }
            };

            btnMaisCaminho.Click += (o, e) =>{
                trocarPagina = true;
                IncluirNoArquivo(REQUEST_CAMINHO);
            };

            btnMaisCidade.Click += (o, e) => {
                trocarPagina = true;
                IncluirNoArquivo(REQUEST_CIDADE);
            };

            canvas = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            imgMapa.SetImageBitmap(canvas);
            LerArquivos();
        }
        private void DesenharCaminho(int origem, int destino, Grafo.Pesos peso)
        {
            DistOriginal[] caminho = grafo.Caminho(origem, destino, peso);
            txtCaminho.Text = ExibirPercurso(origem, destino, caminho);
        }

        private string ExibirPercurso(int inicioDoPercurso, int finalDoPercurso, DistOriginal[] caminho)
        {
            Paint p = new Paint();
            p.StrokeWidth = 15;
            Bitmap btm = canvas.Copy(canvas.GetConfig(), true);
            Canvas c = new Canvas(btm);
            string resultado = "";
            resultado += "Caminho entre " + grafo[inicioDoPercurso].rotulo + " e " + grafo[finalDoPercurso].rotulo+":\n";
            
            p.Color = Color.Red;
            int onde = finalDoPercurso, anterior = 0, tempo = 0, distancia = 0;
            Stack<string> pilha = new Stack<string>();

            c.DrawCircle(bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaX * btm.Width,
                         bucketCidades[grafo[onde].rotulo.ToUpper()].CoordenadaY * btm.Height,
                         20, p);

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
            }

            while (pilha.Count != 0)
            {
                resultado += pilha.Pop();
                if (pilha.Count != 0)
                    resultado += " --> ";
            }

            if ((pilha.Count == 1) && (caminho[finalDoPercurso].peso == Grafo.infinity))
                resultado = "Não existem caminho entre essas cidades.";
            else
                resultado += " --> " + grafo[finalDoPercurso].rotulo;
                
            resultado += "\nTempo: " + tempo + " Distancia: " + distancia + "km";

            imgMapa.SetImageBitmap(btm);

            return resultado;
        }

        private void IncluirNoArquivo(int request)
        {
            Intent intent;
            if (request == REQUEST_CIDADE)
                intent = new Intent(this, typeof(InclusaoView));
            else
                intent = new Intent(this, typeof(InclusaoCaminhoActivity));
            StartActivityForResult(intent, request);
        }

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
                        }
                    }
                    break; 
            }

            trocarPagina = false;
        }

        private void LerArquivos(){

            String sandbox = FilesDir.AbsolutePath;
            string arquivo = System.IO.Path.Combine(sandbox, "Cidades.txt");
            bool existe = System.IO.File.Exists(arquivo);
            FileStream interno = null;
            StreamWriter arqInterno = null;
            if (!existe)
            {
                interno = System.IO.File.OpenWrite(arquivo);
                 arqInterno = new StreamWriter(interno);
            }


            StreamReader leitor = new StreamReader(Assets.Open("Cidades.txt"));
            while (!leitor.EndOfStream){
                Cidade cidade = Cidade.LerRegistro(leitor);
                bucketCidades.Insert(cidade);
                grafo.NovoVertice(cidade.Nome);
                if(!existe)
                    arqInterno.WriteLine(cidade.ParaArquivo());
                
            }

            leitor.Close();

            if(!existe)
            arqInterno.Close();

            arquivo = System.IO.Path.Combine(sandbox, "GrafoTremEspanhaPortugal.txt");
            existe = System.IO.File.Exists(arquivo);
            if (!existe)
            {
                interno = System.IO.File.OpenWrite(arquivo);
                arqInterno = new StreamWriter(interno);
            }

            leitor = new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt"));
            while(!leitor.EndOfStream){
                Caminho caminho = Caminho.LerRegistro(leitor);
                grafo.NovaAresta(bucketCidades[caminho.Origem].Id, bucketCidades[caminho.Destino].Id, caminho.Distancia, caminho.Tempo);
                if (!existe)
                    arqInterno.WriteLine(caminho.ParaArquivo());
                
            }


            if(!existe)
                arqInterno.Close();

            String a = System.IO.File.ReadAllText(arquivo);
            leitor.Close();
        }
        
        protected override void OnStop(){
            base.OnStop();

            if (!trocarPagina)
                EscreverArquivos();
        }

        private void EscreverArquivos()
        {
            String sandbox = FilesDir.AbsolutePath;
            
            cidadesNovas.ParaArquivo(System.IO.Path.Combine(sandbox, "Cidades.txt"));
            caminhosNovos.ParaArquivo(System.IO.Path.Combine(sandbox, "GrafoTremEspanhaPortugal.txt"));
        }
    }
}

