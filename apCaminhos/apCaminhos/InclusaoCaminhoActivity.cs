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
        EditText edtOrigem, edtDestino;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InclusaoCaminho);
            // Create your application here

            //btnAdicionarCaminho = FindViewById<Button>(Resource.Id.btnAdicionar);
            //edtOrigem = FindViewById<EditText>(Resource.Id.edtOrigem);
            //edtDestino = FindViewById<EditText>(Resource.Id.edtDestino);
        }
    }
}