using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace apCaminhos
{
    [Activity(Label = "InclusaoCaminhoActivity")]
    public class InclusaoCaminhoActivity : Activity
    {
        Button btnAdicionarCaminho;
        EditText edtOrigem, edtDestino, edtDistancia, edtTempo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InclusaoCaminho);

            btnAdicionarCaminho = FindViewById<Button>(Resource.Id.btnAdicionar);
            edtOrigem = FindViewById<EditText>(Resource.Id.edtOrigem);
            edtDestino = FindViewById<EditText>(Resource.Id.edtDestino);
            edtDistancia = FindViewById<EditText>(Resource.Id.edtDistancia);
            edtTempo = FindViewById<EditText>(Resource.Id.edtTempo);

            //Envia por meio de uma Intent os valores do novo caminho, sendo a cidade de destino, origem, o tempo e a distância do percurso.
            btnAdicionarCaminho.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(edtOrigem.Text) || string.IsNullOrWhiteSpace(edtDestino.Text)|| string.IsNullOrWhiteSpace(edtDistancia.Text) || string.IsNullOrWhiteSpace(edtTempo.Text))
                    Toast.MakeText(ApplicationContext, "Preencha o nome das cidades de origem e destino.", ToastLength.Short).Show();
                else
                {
                    Intent i = new Intent();
                    i.PutExtra("origem", edtOrigem.Text);
                    i.PutExtra("destino", edtDestino.Text);
                    i.PutExtra("distancia", int.Parse(edtDistancia.Text));
                    i.PutExtra("tempo", int.Parse(edtTempo.Text));
                    SetResult(Result.Ok, i);
                    Finish();
                }
            };
        }
    }
}