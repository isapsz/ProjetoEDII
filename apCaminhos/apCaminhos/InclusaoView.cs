using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

//Amabile Pietrobon Ferreira - 18198
//Isabela Paulino de Souza - 18189
//Gustavo Ferreira Gitzel - 18194

namespace apCaminhos
{ 
    
    [Activity(Label = "InclusaoView")]
    public class InclusaoView : Activity
    {
        ImageView imgMapa;
        EditText edtCidade;
        Button btnSalvar;
        Bitmap bitmap;

        float x = -1, y = -1;
        string nome;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.InclusaoActivity);

            btnSalvar = FindViewById<Button>(Resource.Id.btnSalvar);
            edtCidade = FindViewById<EditText>(Resource.Id.edtNome);
            imgMapa = FindViewById<ImageView>(Resource.Id.img);

            bitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            imgMapa.SetImageBitmap(bitmap);

            imgMapa.Touch += (s, e) =>
            {
                //achando as coordenadas do click em relação ao mapa
                x = (int)e.Event.GetX() * bitmap.Width/imgMapa.Width;
                y = (int)e.Event.GetY() * bitmap.Height / imgMapa.Height;

                Paint p = new Paint();
                Bitmap btm = bitmap.Copy(bitmap.GetConfig(), true);
                Canvas c = new Canvas(btm);

                p.Color = Color.Red;

                c.DrawCircle(x, y, 20, p);

                imgMapa.SetImageBitmap(btm);

                //definindo as coordenadas para serem gravadas no arquivo
                x /= btm.Width;
                y /= btm.Height;
            };

            //Envia, por meio de uma Intent os valores da nova cidade, sendo seu nome e coordenadas.
            btnSalvar.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(edtCidade.Text.ToString()))
                    Toast.MakeText(ApplicationContext, "Preencha o nome da cidade.", ToastLength.Short).Show();
                else
                    if (x == -1 || y == -1)
                    Toast.MakeText(ApplicationContext, "Selecione as coordenadas da cidade.", ToastLength.Short).Show();

                else
                {
                    nome = edtCidade.Text;
                    Intent i = new Intent();
                    i.PutExtra("nome", nome);
                    i.PutExtra("x", x);
                    i.PutExtra("y", y);
                    SetResult(Result.Ok, i);
                    Finish();
                }
            };
        }
    }
}