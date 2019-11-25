using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using Android.Content.Res;

namespace apCaminhos
{
    [Activity(Label = "apCaminho", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnMaisCidade, btnMaisCaminho, btnBuscar;
        ImageView imgMapa;
        EditText edtOrigem, edtDestino;
        Grafo grafo;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);

            btnMaisCaminho = FindViewById<Button>(Resource.Id.btnCaminho);
            btnMaisCidade = FindViewById<Button>(Resource.Id.btnCidade);
            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            imgMapa = FindViewById<ImageView>(Resource.Id.img);
            edtDestino = FindViewById<EditText>(Resource.Id.edtDestino);
            edtOrigem = FindViewById<EditText>(Resource.Id.edtOrigem);

            btnBuscar.Click += (o, e) => {
                
            };

            LerCaminhos();
        }

        public void LerCaminhos(){
            StreamReader leitor = new StreamReader(Assets.Open("Cidades.txt"));
            while (!leitor.EndOfStream){
                string nomeCidade = Cidade.LerRegistro(leitor).Nome;
                grafo.NovoVertice(nomeCidade);
            }

            leitor.Close();

            leitor = new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt"));
            while(!leitor.EndOfStream){
                Caminho caminho = Caminho.LerRegistro(leitor);
            }

            leitor.Close();
        }

        /*
         var linearLayout = new LinearLayout(this);
linearLayout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

var scrollView = new ScrollView(this);
scrollView.LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

scrollView.AddView(linearLayout);

SetContentView(scrollView);
        
        const int TAMANHOX = 4096, TAMANHOY = 2048;   
        
      
        int selecionado;

       
        List<PilhaLista<Caminho>> listaCaminhos;

      
        Arvore<Cidade> arvore;


        Grafo grafo;

        
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (lsbOrigem.SelectedIndex >= 0 && lsbDestino.SelectedIndex >= 0)
            {
                int origem = int.Parse(lsbOrigem.SelectedItem.ToString().Substring(0, 2));

                int destino = int.Parse(lsbDestino.SelectedItem.ToString().Substring(0, 2));

                if (destino == origem)
                    MessageBox.Show("Selecione cidades diferentes!", "Viagem inválida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else
                {
                    AcharCaminhos(origem, destino);

                    dgvMelhorCaminho.RowCount = dgvMelhorCaminho.ColumnCount = dgvCaminhoEncontrado.RowCount = dgvCaminhoEncontrado.ColumnCount = 0;

                    if (listaCaminhos.Count != 0)
                        MostrarCaminhos();
                    else
                    {
                        selecionado = -1;
                        pbMapa.Invalidate();
                        dgvMelhorCaminho.RowCount = dgvMelhorCaminho.ColumnCount = 0;
                        MessageBox.Show("Não existe caminho entre essas cidades!", "Viagem inválida", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


       
     
        private void AcharCaminhos(int origem, int destino)
        {
            listaCaminhos = new List<PilhaLista<Caminho>>();

            int menorDistancia = int.MaxValue, disAtual = 0;
            PilhaLista<Caminho> caminhoAtual = new PilhaLista<Caminho>();

            PilhaLista<Caminho> aux = new PilhaLista<Caminho>();

            bool[] jaPassou = new bool[23];
            for (int i = 0; i < 23; i++)
                jaPassou[i] = false;

            int atual = origem;

            bool acabou = false;

            while (!acabou)
            {
                int tamanhoAnterior = aux.Tamanho();
                for (int i = 0; i < 23; i++)
                    if (grafo[atual, i] != 0 && !jaPassou[i])
                        aux.Empilhar(new Caminho(atual, i, grafo[atual, i]));

                if (!aux.EstaVazia() && tamanhoAnterior == aux.Tamanho())
                {
                    Caminho cam = caminhoAtual.Desempilhar();
                    disAtual -= cam.Distancia;
                    jaPassou[cam.IdDestino] = true;
                }

                if (aux.EstaVazia())
                    acabou = true;
                else
                {
                    Caminho c = aux.Desempilhar();

                    while (!caminhoAtual.EstaVazia() && caminhoAtual.OTopo().IdDestino != c.IdOrigem)
                    {
                        Caminho cam = caminhoAtual.Desempilhar();
                        disAtual -= cam.Distancia;
                        jaPassou[cam.IdDestino] = false;
                    }

                    caminhoAtual.Empilhar(c);
                    disAtual += c.Distancia;

                    if (c.IdDestino != destino)
                    {
                        jaPassou[c.IdOrigem] = true;
                        atual = c.IdDestino;
                    }
                    else
                    {
                        listaCaminhos.Add(caminhoAtual.Clone());
                        if (disAtual < menorDistancia)
                        {
                            menor = listaCaminhos.Count - 1;
                            menorDistancia = disAtual;
                        }

                        if (aux.EstaVazia())
                            acabou = true;
                        else
                        {
                            Caminho retorno = aux.Desempilhar();
                     
                            while (!caminhoAtual.EstaVazia() && caminhoAtual.OTopo().IdDestino != retorno.IdOrigem)
                            {
                                Caminho cam = caminhoAtual.Desempilhar();
                                disAtual -= cam.Distancia;
                                jaPassou[cam.IdDestino] = false;
                            }

                            caminhoAtual.Empilhar(retorno);
                            jaPassou[retorno.IdDestino] = true;
                            disAtual += retorno.Distancia;

                            while(retorno.IdDestino == destino && !acabou)
                            {
                                listaCaminhos.Add(caminhoAtual.Clone());

                                if (disAtual < menorDistancia)
                                {
                                    menor = listaCaminhos.Count - 1;
                                    menorDistancia = disAtual;
                                }

                                if (!aux.EstaVazia())
                                {
                                    retorno = aux.Desempilhar();
                                    while (!caminhoAtual.EstaVazia() && caminhoAtual.OTopo().IdDestino != retorno.IdOrigem)
                                    {
                                        Caminho cam = caminhoAtual.Desempilhar();
                                        disAtual -= cam.Distancia;
                                        jaPassou[cam.IdDestino] = false;
                                    }

                                    caminhoAtual.Empilhar(retorno);
                                    disAtual += retorno.Distancia;
                                }
                                else
                                    acabou = true;
                            }

                            atual = retorno.IdDestino;
                        }
                    }
                }
            }
        }

       
        private void MostrarCaminhos()
        {
            foreach (PilhaLista<Caminho> caminho in listaCaminhos)
            {
                int posicao = 0;
                PilhaLista<Caminho> aux = caminho.Clone();
                aux.Inverter();

                if (dgvCaminhoEncontrado.RowCount == menor)
                {
                    dgvMelhorCaminho.RowCount++;
                    dgvMelhorCaminho.ColumnCount = aux.Tamanho() + 1;
                }

                dgvCaminhoEncontrado.RowCount++;


                if (dgvCaminhoEncontrado.ColumnCount <= aux.Tamanho())
                    dgvCaminhoEncontrado.ColumnCount = aux.Tamanho() + 1;

                while (!aux.EstaVazia())
                {
                    Caminho c = aux.Desempilhar();
                    if (dgvCaminhoEncontrado.RowCount - 1 == menor)
                        ExibirDgv(dgvMelhorCaminho, c, posicao);

                    ExibirDgv(dgvCaminhoEncontrado, c, posicao);
                    posicao++;
                }
            }

            selecionado = menor;
            dgvCaminhoEncontrado.Rows[selecionado].Selected = true;
            pbMapa.Invalidate();
        }

     
          
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

