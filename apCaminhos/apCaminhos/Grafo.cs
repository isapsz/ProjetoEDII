using System;
using System.Collections.Generic;

class Grafo
{
    private const int NUM_VERTICES = 20;
    private Vertice[] vertices;
    private int[,] adjMatrix;
    int numVerts;

    /// DJIKSTRA
    DistOriginal[] percurso;
    int infinity = 1000000;
    int verticeAtual;   // global usada para indicar o vértice atualmente sendo visitado 
    int doInicioAteAtual;   // global usada para ajustar menor caminho com Djikstra
    int nTree;

    public Grafo()
    {
        vertices = new Vertice[NUM_VERTICES];
        adjMatrix = new int[NUM_VERTICES, NUM_VERTICES];
        numVerts = 0;
        nTree = 0;

        for (int j = 0; j < NUM_VERTICES; j++)      // zera toda a matriz
            for (int k = 0; k < NUM_VERTICES; k++)
                adjMatrix[j, k] = infinity; // distância tão grande que não existe

        percurso = new DistOriginal[NUM_VERTICES];
    }

    public void NovoVertice(string valor)
    {
        vertices[numVerts] = new Vertice(valor);
        numVerts++;
    }

    public void NovaAresta(int origem, int destino)
    {
        adjMatrix[origem, destino] = 1;
    }

    public void IncluirValorAresta(string origem, string destino, int dist){
        int o = -1, d = -1;
        for (int i = 0; i < vertices.Length; i++)
            if (vertices[i].Equals(origem))
                o = i;
            else if (vertices[i].Equals(destino))
                d = i;
        NovaAresta(o, d, dist);
    }


    public void NovaAresta(int origem, int destino, int peso)
    {
        if (origem < 0 || destino < 0 || origem > vertices.Length - 1 || destino > vertices.Length - 1)
            throw new Exception("Origem ou Destino Inválidos!!!");
        adjMatrix[origem, destino] = peso;
    }

    public void ExibirVertice(int v)
    {
        Console.Write(vertices[v].rotulo + " ");
    }

    public int SemSucessores() 	// encontra e retorna a linha de um vértice sem sucessores
    {
        bool temAresta;
        for (int linha = 0; linha < numVerts; linha++)
        {
            temAresta = false;
            for (int col = 0; col < numVerts; col++)
                if (adjMatrix[linha, col] != infinity)
                {
                    temAresta = true;
                    break;
                }
            if (!temAresta)
                return linha;
        }
        return -1;
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
                adjMatrix[row, col] = adjMatrix[row + 1, col];  // desloca para excluir
    }
    private void moverColunas(int col, int length)
    {
        if (col != numVerts - 1)
            for (int row = 0; row < length; row++)
                adjMatrix[row, col] = adjMatrix[row, col + 1]; // desloca para excluir
    }

    public String OrdenacaoTopologica()
    {
        Stack<String> gPilha = new Stack<String>(); // para guardar a sequência de vértices
        int origVerts = numVerts;
        while (numVerts > 0)
        {
            int currVertex = SemSucessores();
            if (currVertex == -1)
                return "Erro: grafo possui ciclos.";
            gPilha.Push(vertices[currVertex].rotulo);   // empilha vértice
            removerVertice(currVertex);
        }
        String resultado = "Sequência da Ordenação Topológica: ";
        while (gPilha.Count > 0)
            resultado += gPilha.Pop() + " ";    // desempilha para exibir
        return resultado;
    }

    private int ObterVerticeAdjacenteNaoVisitado(int v)
    {
        for (int j = 0; j <= numVerts - 1; j++)
            if ((adjMatrix[v, j] != infinity) && (!vertices[j].foiVisitado))
                return j;
        return -1;
    }

    public void PercursoEmProfundidade()
    {
        Stack<int> gPilha = new Stack<int>(); 
        vertices[0].foiVisitado = true;
        ExibirVertice(0);
        gPilha.Push(0);
        int v;
        while (gPilha.Count > 0)
        {
            v = ObterVerticeAdjacenteNaoVisitado(gPilha.Peek());
            if (v == -1)
                gPilha.Pop();
            else
            {
                vertices[v].foiVisitado = true;
                gPilha.Push(v);
            }
        }
        for (int j = 0; j <= numVerts - 1; j++)
            vertices[j].foiVisitado = false;
    }


    public void PercursoEmProfundidadeRec(int[,] adjMatrix, int numVerts, int part)
    {
        int i;
        //fazer algo aqui
        vertices[part].foiVisitado = true;
        for (i = 0; i < numVerts; ++i)
            if (adjMatrix[part, i] != infinity && !vertices[i].foiVisitado)
                PercursoEmProfundidadeRec(adjMatrix, numVerts, i);
    }

    public void percursoPorLargura()
    {
        Queue<int> gQueue = new Queue<int>();
        vertices[0].foiVisitado = true;
        ExibirVertice(0);
        gQueue.Enqueue(0);
        int vert1, vert2;
        while (gQueue.Count >0 )
        {
            vert1 = gQueue.Dequeue();
            vert2 = ObterVerticeAdjacenteNaoVisitado(vert1);
            while (vert2 != -1)
            {
                vertices[vert2].foiVisitado = true;
                ExibirVertice(vert2);
                gQueue.Enqueue(vert2);
                vert2 = ObterVerticeAdjacenteNaoVisitado(vert1);
            }
        }
        for (int i = 0; i < numVerts; i++)
            vertices[i].foiVisitado = false;
    }

    public void ArvoreGeradoraMinima(int primeiro)
    {
        Stack<int> gPilha = new Stack<int>(); // para guardar a sequência de vértices
        vertices[primeiro].foiVisitado = true;
        gPilha.Push(primeiro);
        int currVertex, ver;
        while (gPilha.Count > 0)
        {
            currVertex = gPilha.Peek();
            ver = ObterVerticeAdjacenteNaoVisitado(currVertex);
            if (ver == -1)
                gPilha.Pop();
            else
            {
                vertices[ver].foiVisitado = true;
                gPilha.Push(ver);
                ExibirVertice(currVertex);
                ExibirVertice(ver);
            }
        }
        for (int j = 0; j <= numVerts - 1; j++)
            vertices[j].foiVisitado = false;
    }

    public string Caminho(int inicioDoPercurso, int finalDoPercurso)
    {
        for (int j = 0; j < numVerts; j++)
            vertices[j].foiVisitado = false;

        vertices[inicioDoPercurso].foiVisitado = true;
        for (int j = 0; j < numVerts; j++)
        {
            // anotamos no vetor percurso a distância entre o inicioDoPercurso e cada vértice
            // se não há ligação direta, o valor da distância será infinity
            int tempDist = adjMatrix[inicioDoPercurso, j];
            percurso[j] = new DistOriginal(inicioDoPercurso, tempDist);
        }

        for (int nTree = 0; nTree < numVerts; nTree++)
        {
            // Procuramos a saída não visitada do vértice inicioDoPercurso com a menor distância
            int indiceDoMenor = ObterMenor();

            // e anotamos essa menor distância
            int distanciaMinima = percurso[indiceDoMenor].distancia;


            // o vértice com a menor distância passa a ser o vértice atual
            // para compararmos com a distância calculada em AjustarMenorCaminho()
            verticeAtual = indiceDoMenor;
            doInicioAteAtual = percurso[indiceDoMenor].distancia;

            // visitamos o vértice com a menor distância desde o inicioDoPercurso
            vertices[verticeAtual].foiVisitado = true;
            AjustarMenorCaminho();
        }

        return ExibirPercursos(inicioDoPercurso, finalDoPercurso);
    }

    public int ObterMenor()
    {
        int distanciaMinima = infinity;
        int indiceDaMinima = 0;
        for (int j = 0; j < numVerts; j++)
            if (!(vertices[j].foiVisitado) && (percurso[j].distancia < distanciaMinima))
            {
                distanciaMinima = percurso[j].distancia;
                indiceDaMinima = j;
            }
        return indiceDaMinima;
    }

    public void AjustarMenorCaminho()
    {
        for (int coluna = 0; coluna < numVerts; coluna++)
            if (!vertices[coluna].foiVisitado)       // para cada vértice ainda não visitado
            {
                int atualAteMargem = adjMatrix[verticeAtual, coluna];

                int doInicioAteMargem = doInicioAteAtual + atualAteMargem;

                int distanciaDoCaminho = percurso[coluna].distancia;
                if (doInicioAteMargem < distanciaDoCaminho)
                {
                    percurso[coluna].verticePai = verticeAtual;
                    percurso[coluna].distancia = doInicioAteMargem;
                    ExibirTabela();
                }
            }
    }

    public void ExibirTabela()
    {
        string dist = "";
        for (int i = 0; i < numVerts; i++)
        {
            if (percurso[i].distancia == infinity)
                dist = "inf";
            else
                dist = Convert.ToString(percurso[i].distancia);

            ///vertices[i].rotulo + "\t" + vertices[i].foiVisitado +
                 // "\t\t" + dist + "\t" + vertices[percurso[i].verticePai].rotulo);
        }
    }

    public string ExibirPercursos(int inicioDoPercurso, int finalDoPercurso)
    {
        string linha = "", resultado = "";
        for (int j = 0; j < numVerts; j++)
        {
            linha += vertices[j].rotulo + "=";
            if (percurso[j].distancia == infinity)
                linha += "inf";
            else
                linha += percurso[j].distancia;
            string pai = vertices[percurso[j].verticePai].rotulo;
            linha += "(" + pai + ") ";
        }
        //lista.Items.Add("Caminho entre " + vertices[inicioDoPercurso].rotulo +
                                  // " e " + vertices[finalDoPercurso].rotulo);
       

        int onde = finalDoPercurso;
        Stack<string> pilha = new Stack<string>();

        int cont = 0;
        while (onde != inicioDoPercurso)
        {
            onde = percurso[onde].verticePai;
            pilha.Push(vertices[onde].rotulo);
            cont++;
        }

        while (pilha.Count != 0)
        {
            resultado += pilha.Pop();
            if (pilha.Count != 0)
                resultado += " --> ";
        }

        if ((cont == 1) && (percurso[finalDoPercurso].distancia == infinity))
            resultado = "Não há caminho";
        else
            resultado += " --> " + vertices[finalDoPercurso].rotulo;
        return resultado;
    }


}
