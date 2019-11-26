using Android.Content;
using Android.Graphics;
using Android.Views;
using System;
using System.Collections.Generic;

public class Mapa : View
{
    private Paint paint;

    public Mapa(Context context) : base(context)
    {
    }

    protected override void OnDraw(Canvas canvas)
    {
        base.OnDraw(canvas);
        paint = new Paint();
        canvas.DrawText("oi", 12, 23, paint);
    }

    public void DesenharCaminho(Canvas canvas, Cidade origem, Cidade destino)
    {
        base.OnDraw(canvas);
        paint = new Paint();
        canvas.DrawCircle(origem.CoordenadaX*canvas.Width, origem.CoordenadaY*canvas.Height, 25 ,paint);
    }
}