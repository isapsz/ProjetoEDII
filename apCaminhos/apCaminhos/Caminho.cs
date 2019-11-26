using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ISABELA PAULINO DE SOUZA 18189, GUSTAVO FERRREIRA GITZEL 18194 e AMABILE PIETROBON FERREIRA 18198

class Caminho  : IComparable<Caminho>
{
    /*
     Atributos inteiros constantes que armazenam os inicios e tamanhos de cada um dos atributos da classe quando estes
     estiverem em um arquivo texto formatado corretamente.
    */
    const int inicioOrigem = 0,
              tamanhoOrigem = 16,
              inicioDestino = inicioOrigem + tamanhoOrigem,
              tamanhoDestino = 16,
              inicioDistancia = inicioDestino + tamanhoDestino,
              tamanhoDistancia = 5,
              inicioTempo = inicioDistancia + tamanhoDistancia;


    /*
     Atributos inteiros que representam quanto tempo se gasta para percorrê o caminho e qual
     a distância entre os dois pontos.
    */
    protected int distancia, tempo;

    /*
        Atributos que representam o nome da cidade de origem e destino de um caminho. 
    */
    protected string destino, origem;


    /*
     Sobrecarga do construtor que não recebe nenhum parâmetro e inicia os atributos com valores padrões.
    */
    public Caminho()
    {
        destino =  origem = null;
        tempo =  distancia = 0;
    }

    /*
     Sobrecarga do construtor que não recebe como parâmetro todos os atributos da classe e os instância com base nos parâmetros.
     @params os atributos inteiros origem e destino do caminho, distancia entre esses dois pontos e o tempo da viagem.
    */
    public Caminho(string o, string d, int distancia, int tempo)
    {
        Origem = o;
        Destino = d;
        Distancia = distancia;
        Tempo = tempo;
    }

    /*
      Sobrecarga do construtor que não recebe como parâmetro alguns atributos da classe e os instância com base nos parâmetros.
      @params os atributos inteiros origem e destino do caminho e a distancia entre esses dois pontos.
    */
    public Caminho(string origem, string destino, int distancia)
    {
        Origem = origem;
        Destino = destino;
        Distancia = distancia;
    }

    /*
     Propriedade que retorna e altera o valor do atributo texto destino.
   */
    public string Destino
    {
        get => destino;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                destino = value;
        }
    }

    /*
     Propriedade que retorna e altera o valor do atributo texto origem.
   */
    public string Origem
    {
        get => origem;
        set
        {
            if(!string.IsNullOrWhiteSpace(value))
            origem = value;
        }
    }
    /*
      Propriedade que retorna e altera o valor  do atributo inteiro que guarda a distancia entre os dois pontos.
    */
    public int Distancia
    {
        get => distancia;
        set
        {
            if (value > 0)
                distancia = value;
        }
    }

    /*
      Propriedade que retorna e altera o valor  do atributo inteiro que guarda o tempo que se gasta para percorrer o caminho
      entre os dois pontos.
    */
    public int Tempo
    {
        get => tempo;
        set
        {
            if (value >= 0)
                tempo = value;
        }
    }

     /*
     Método que lê uma linha de um StreamReader de um arquivo que contém os caminhos e retorna um Caminho com base na linha
     lida daquele arquivo.
     @params o StreamReader do arquivo que está sendo lido.
     @return um Caminho com as informações contidas naquelas linha lida do arquivo.
    */
    public static Caminho LerRegistro(StreamReader arq)
    {
        Caminho ret = null;
        try
        {
            if (!arq.EndOfStream)
            {
                string linha = arq.ReadLine();

                ret = new Caminho(linha.Substring(inicioOrigem, tamanhoOrigem).Trim(),
                                  linha.Substring(inicioDestino, tamanhoDestino).Trim(),
                                  int.Parse(linha.Substring(inicioDistancia,tamanhoDistancia)),
                                  int.Parse(linha.Substring(inicioTempo)));
            }
        }
        catch (Exception erro)
        {
            throw new Exception(erro.Message);
        }
        return ret;
    }

    /*
      Método que compara dois caminhos com base no idDestino e origem e se não for o mesmo caminho retorna a diferença entre as
      distancias.
      @params outro Caminho que será usado para comparar.
      @return um int que é a diferença entre as duas distâncias e se for o mesmo caminho retorna 0.
    */
    public int CompareTo(Caminho other)
    {
        if (destino.Equals(other.destino) && origem.Equals(other.origem) && distancia.Equals(other.distancia))
            return 0;

        return this.distancia - other.distancia;
    }
}

