using System;
using System.Collections.Generic;
using System.IO;

class Grafo
{
    public enum Pesos {tempo, distancia};

    private class Peso
    {
        int distancia, tempo;

        public Peso(int distancia, int tempo)
        {
            Distancia = distancia;
            Tempo = tempo;
        }

        public int Distancia { get => distancia; set => distancia = value; }
        public int Tempo { get => tempo; set => tempo = value; }

    }

    private const int NUM_VERTICES = 100;
    private Vertice[] vertices;
    private Peso[,] adjMatriz;
    int numVerts;

    /// DJIKSTRA
    DistOriginal[] percurso;
    int infinity = int.MaxValue;
    int verticeAtual;   // global usada para indicar o vértice atualmente sendo visitado 
    int doInicioAteAtual;   // global usada para ajustar menor caminho com Djikstra

    public int NumVerts {
        get => numVerts;
    }

    public Grafo()
    {
        vertices = new Vertice[NUM_VERTICES];
        adjMatriz = new Peso[NUM_VERTICES, NUM_VERTICES];
        numVerts = 0;

        for (int j = 0; j < NUM_VERTICES; j++)      // zera toda a matriz
            for (int k = 0; k < NUM_VERTICES; k++)
            {
                adjMatriz[j, k] = new Peso(infinity, infinity);
            }

        percurso = new DistOriginal[NUM_VERTICES];
    }

    public void NovoVertice(string valor)
    {
        vertices[numVerts] = new Vertice(valor);
        numVerts++;
    }

    /*public void IncluirValorAresta(int idOrigem,int idDestino, int distancia, int tempo){
       int o = -1, d = -1;
        for (int i = 0; i < numVerts; i++)
            if (vertices[i].rotulo.Equals(caminho.Origem))
                o = i;
            else if (vertices[i].rotulo.Equals(caminho.Destino))
                d = i;
        NovaAresta(caminho);
    }*/


    public void NovaAresta(int idOrigem, int idDestino, int distancia, int tempo)
    {
        if (idOrigem < 0 || idDestino < 0 || idOrigem > vertices.Length - 1 || idDestino > vertices.Length - 1)
            throw new Exception("Origem ou Destino Inválidos!!!");
        adjMatriz[idOrigem, idDestino].Distancia = distancia;
        adjMatriz[idOrigem, idDestino].Tempo = tempo;
    }
    

    public void removerVertice(int vert)
    {
        if (vert != numVerts - 1)
        {
            for (int j = vert; j < numVerts - 1; j++)   // remove vértice do vetor
                vertices[j] = vertices[j + 1];

            // remove vértice da matriz
            for (int row = vert; row < numVerts; row++)
                moverLinhas(row, numVerts - 1);
            for (int col = vert; col < numVerts; col++)
                moverColunas(col, numVerts - 1);
        }
        numVerts--;
    }
    private void moverLinhas(int row, int length)
    {
        if (row != numVerts - 1)
            for (int col = 0; col < length; col++)
                adjMatriz[row, col] = adjMatriz[row + 1, col];  // desloca para excluir
    }
    private void moverColunas(int col, int length)
    {
        if (col != numVerts - 1)
            for (int row = 0; row < length; row++)
                adjMatriz[row, col] = adjMatriz[row, col + 1]; // desloca para excluir
    }
    

    public string Caminho(int inicioDoPercurso, int finalDoPercurso, Pesos opcao)
    {
        for (int j = 0; j < numVerts; j++)
            vertices[j].foiVisitado = false;

        vertices[inicioDoPercurso].foiVisitado = true;

        for (int j = 0; j < numVerts; j++)
        {
            // anotamos no vetor percurso a distância entre o inicioDoPercurso e cada vértice
            // se não há ligação direta, o valor da distância será infinity
            int tempDist = (opcao == Pesos.distancia) ? adjMatriz[inicioDoPercurso, j].Distancia : adjMatriz[inicioDoPercurso, j].Tempo;
            percurso[j] = new DistOriginal(inicioDoPercurso, tempDist);
        }

        for (int nTree = 0; nTree < numVerts; nTree++)
        {
            // Procuramos a saída não visitada do vértice inicioDoPercurso com a menor distância
            int indiceDoMenor = ObterMenor();

            // e anotamos essa menor distância
            int distanciaMinima = percurso[indiceDoMenor].peso;

            // o vértice com a menor distância passa a ser o vértice atual
            // para compararmos com a distância calculada em AjustarMenorCaminho()
            verticeAtual = indiceDoMenor;
            doInicioAteAtual = percurso[indiceDoMenor].peso;

            // visitamos o vértice com a menor distância desde o inicioDoPercurso
            vertices[verticeAtual].foiVisitado = true;
            AjustarMenorCaminho(opcao);
        }

        return ExibirPercurso(inicioDoPercurso, finalDoPercurso);
    }

    public int ObterMenor()
    {
        int pesoMinimo = infinity;
        int indiceMenor = 0;
        for (int j = 0; j < numVerts; j++)
            if (!(vertices[j].foiVisitado) && (percurso[j].peso < pesoMinimo))
            {
                pesoMinimo = percurso[j].peso;
                indiceMenor = j;
            }
        return indiceMenor;
    }

    public void AjustarMenorCaminho(Pesos opcao = Pesos.distancia)
    {
        for (int coluna = 0; coluna < numVerts; coluna++)
            if (!vertices[coluna].foiVisitado)       // para cada vértice ainda não visitado
            {
                int atualAteMargem = (opcao == Pesos.distancia) ? adjMatriz[verticeAtual, coluna].Distancia : adjMatriz[verticeAtual, coluna].Tempo;

                int doInicioAteMargem = doInicioAteAtual + atualAteMargem;

                int distanciaDoCaminho = percurso[coluna].peso;

                if (doInicioAteMargem < distanciaDoCaminho)
                {
                    percurso[coluna].verticePai = verticeAtual;
                    percurso[coluna].peso = doInicioAteMargem;
                }
            }
    }


    public string ExibirPercurso(int inicioDoPercurso, int finalDoPercurso)
    {
        string resultado = "";

        resultado += "Caminho entre " + vertices[inicioDoPercurso].rotulo + " e " + vertices[finalDoPercurso].rotulo;

        int onde = finalDoPercurso, anterior = 0, tempo= 0, distancia= 0;
        Stack<string> pilha = new Stack<string>();

        while (onde != inicioDoPercurso)
        {
            anterior = onde;
            onde = percurso[onde].verticePai;

            tempo += adjMatriz[onde, anterior].Tempo;
            distancia += adjMatriz[onde, anterior].Distancia;

            pilha.Push(vertices[onde].rotulo);
        }

        while (pilha.Count != 0)
        {
            resultado += pilha.Pop();
            if (pilha.Count != 0)
                resultado += " --> ";
        }

        if ((pilha.Count == 1) && (percurso[finalDoPercurso].peso == infinity))
            resultado = "Não há caminho";
        else
            resultado += " --> " + vertices[finalDoPercurso].rotulo;

        resultado += "\nTempo: " + tempo + "Distancia: " + distancia+ "km";

        return resultado;

        /* for (int j = 0; j < numVerts; j++)
         {
             linha += vertices[j].rotulo + "=";
             if (percurso[j].peso == infinity)
                 linha += "inf";
             else
                 linha += percurso[j].peso;
             string pai = vertices[percurso[j].verticePai].rotulo;
             linha += "(" + pai + ") ";
         }*/


        //lista.Items.Add("Caminho entre " + vertices[inicioDoPercurso].rotulo +
        // " e " + vertices[finalDoPercurso].rotulo);


    }


}
