using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using Android.Views;
using Android.Graphics;
using Android.Content;
using Android.Runtime;

namespace apCaminhos
{
    [Activity(Label = "apCaminho", MainLauncher = true)]
    public class MainActivity : Activity
    {
        const int REQUEST_CIDADE = 1, REQUEST_CAMINHO = 2;

        Button btnMaisCidade, btnMaisCaminho, btnBuscar;
        View imgMapa;
        EditText edtOrigem, edtDestino;
        Grafo grafo;
        BucketHash bucketCidades;
        Canvas canvas;
        RadioButton rbTempo, rbDistancia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            bucketCidades = new BucketHash();
            grafo = new Grafo();

            btnMaisCaminho = FindViewById<Button>(Resource.Id.btnCaminho);
            btnMaisCidade = FindViewById<Button>(Resource.Id.btnCidade);
            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            imgMapa = FindViewById<View>(Resource.Id.img);
            edtDestino = FindViewById<EditText>(Resource.Id.edtDestino);
            edtOrigem = FindViewById<EditText>(Resource.Id.edtOrigem);
            rbDistancia = FindViewById<RadioButton>(Resource.Id.rbDistancia);
            rbTempo = FindViewById<RadioButton>(Resource.Id.rbTempo);
            
            btnBuscar.Click += (o, e) => {
                Cidade origem = bucketCidades[new Cidade(edtOrigem.Text)];
                Cidade destino = bucketCidades[new Cidade(edtDestino.Text)];
                
                DesenharCaminho(origem.Id, destino.Id, (rbDistancia.Checked)? Grafo.Pesos.distancia : Grafo.Pesos.tempo);
            };

            btnMaisCaminho.Click += (o, e) =>{
                IncluirNoArquivo(REQUEST_CIDADE);
            };

            btnMaisCidade.Click += (o, e) => {
                IncluirNoArquivo(REQUEST_CAMINHO);
            };

            LerArquivos();
        }

        private void RbDistancia_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void DesenharCaminho(int origem, int destino, Grafo.Pesos peso)
        {
            string caminho = grafo.Caminho(origem, destino, peso);
        }

        private void IncluirNoArquivo(int request)
        {
            Intent intent;
            if (request == REQUEST_CIDADE)
                intent = new Intent(this, typeof(InclusaoView));
            else
                intent = new Intent(this, typeof(InclusaoCiade));
            StartActivityForResult(intent, request);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            switch(requestCode)
            {
                case REQUEST_CIDADE:
                    if(resultCode == Result.Ok)
                    {
                        Cidade nova = new Cidade(grafo.NumVerts, data.GetStringExtra("nome"), data.GetIntExtra("x", 0), data.GetIntExtra("y", 0));
                        grafo.NovoVertice(nova.Nome);
                        bucketCidades.Insert(nova);
                    }
                    break;

                case REQUEST_CAMINHO:
                    if (resultCode == Result.Ok)
                    {
                        //Caminho nova = new Caminho(grafo.NumVerts, data.GetStringExtra("nome"), data.GetIntExtra("x", 0), data.GetIntExtra("y", 0));
                        //grafo.NovaAresta();
                    }
                    break; 
            }
        }

        private void LerArquivos(){
            StreamReader leitor = new StreamReader(Assets.Open("Cidades.txt"));
            while (!leitor.EndOfStream){
                Cidade cidade = Cidade.LerRegistro(leitor);
                bucketCidades.Insert(cidade);
                grafo.NovoVertice(cidade.Nome);
            }

            leitor.Close();

            leitor = new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt"));
            while(!leitor.EndOfStream){
                Caminho caminho = Caminho.LerRegistro(leitor);
                grafo.NovaAresta(bucketCidades[caminho.Origem].Id, bucketCidades[caminho.Destino].Id, caminho.Distancia, caminho.Tempo);
            }

            leitor.Close();
        }

        protected override void OnStop(){
            EscreverArquivos();

            base.OnStop();
        }

        private void EscreverArquivos()
        {
            StreamWriter escritor = new StreamWriter(Assets.Open("Cidades.txt"));

            //percorrer o buckethash
            
            escritor.Close();

           
        }
        

      /*
     
          
        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            arvore.PreOrdem((Cidade c) =>
            {
                float coordenadaX = c.CoordenadaX * pbMapa.Width / TAMANHOX;
                float coordenadaY = c.CoordenadaY * pbMapa.Height / TAMANHOY;
                g.FillEllipse(
                 new SolidBrush(Color.Black),
                 coordenadaX, coordenadaY, 10f, 10f
               );
                g.DrawString(c.Nome, new Font("Courier New", 8, FontStyle.Bold),
                             new SolidBrush(Color.FromArgb(32, 32, 32)), coordenadaX + 12, coordenadaY - 10);
            });

            if (selecionado >= 0)
            {
                PilhaLista<Caminho> aux = listaCaminhos[selecionado].Clone();
               
                while (!aux.EstaVazia())
                {
                    Caminho possivelCaminho = aux.Desempilhar();

                    Cidade origem = arvore.ExisteDado(new Cidade(possivelCaminho.IdOrigem));
                    Cidade destino = arvore.ExisteDado(new Cidade(possivelCaminho.IdDestino));
                    using (var pen = new Pen(Color.FromArgb(211, 47, 47), 4))
                    {
                        
                        int origemX = origem.CoordenadaX * pbMapa.Width / TAMANHOX + 5;
                        int origemY = origem.CoordenadaY * pbMapa.Height / TAMANHOY + 3;
                        int destinoX = destino.CoordenadaX * pbMapa.Width / TAMANHOX +3;
                        int destinoY = destino.CoordenadaY * pbMapa.Height / TAMANHOY +5;


                        
                        if (destinoX - origemX > 2 * pbMapa.Width / 4)
                        {
                            g.DrawLine(pen, origemX, origemY, 0, origemY);
                            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            g.DrawLine(pen, pbMapa.Width, origemY, destinoX, destinoY);
                        }
                        else
                        {
                            pen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                            g.DrawLine(pen, origemX,origemY, destinoX,  destinoY);
                        }
                    }
                }
            }
        }   
         */
    }
}

