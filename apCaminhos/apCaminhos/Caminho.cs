﻿using System;
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
              tamanhoOrigem = 2,
              inicioDestino = inicioOrigem + tamanhoOrigem,
              tamanhoDestino = 2,
              inicioDistancia = inicioDestino + tamanhoDestino,
              tamanhoDistancia = 4,
              inicioTempo = inicioDistancia + tamanhoDistancia,
              tamanhoTempo = 5;
              /*inicioCusto = inicioTempo + tamanhoTempo,
              tamanhoCusto = 5;*/


    /*
     Atributos inteiros que representam a origem e destino do caminho, quanto tempo se gasta para percorrê-lo e qual
     a distância entre os dois pontos.
    */
    protected int idDestino, idOrigem, distancia, tempo, custo;  // talvez tenha custo


    /*
     Sobrecarga do construtor que não recebe nenhum parâmetro e inicia os atributos com valores padrões.
    */
    public Caminho()
    {
        idDestino =  idOrigem = 0;
        tempo = custo = distancia = 0;
    }

    /*
     Sobrecarga do construtor que não recebe como parâmetro todos os atributos da classe e os instância com base nos parâmetros.
     @params os atributos inteiros origem e destino do caminho, distancia entre esses dois pontos e o tempo da viagem.
    */
    public Caminho(int o, int d, int distancia, int tempo)
    {
        IdOrigem = o;
        IdDestino = d;
        Distancia = distancia;
        Tempo = tempo;
    }

    /*
      Sobrecarga do construtor que não recebe como parâmetro alguns atributos da classe e os instância com base nos parâmetros.
      @params os atributos inteiros origem e destino do caminho e a distancia entre esses dois pontos.
    */
    public Caminho(int idOrigem, int idDestino, int distancia)
    {
        IdOrigem = idOrigem;
        IdDestino = idDestino;
        Distancia = distancia;
    }

    /*
     Propriedade que retorna e altera o valor do atributo texto destino.
   */
    public int IdDestino
    {
        get => idDestino;
        set
        {
            if (value > 0)
                idDestino = value;
        }
    }

    /*
     Propriedade que retorna e altera o valor do atributo texto origem.
   */
    public int IdOrigem
    {
        get => idOrigem;
        set
        {
            if(value > 0)
            idOrigem = value;
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
      Propriedade que retorna e altera o valor do atributo inteiro que guarda quanto que se gasta para percorrer o caminho
      entre os dois pontos.
    */
    public int Custo
    {
        get => custo;
        set
        {
            if (value >= 0)
                custo = value;
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

                ret = new Caminho(int.Parse(linha.Substring(inicioOrigem, tamanhoOrigem)),
                                  int.Parse(linha.Substring(inicioDestino, tamanhoDestino)),
                                  int.Parse(linha.Substring(inicioDistancia,tamanhoDistancia)),
                                  int.Parse(linha.Substring(inicioTempo, tamanhoTempo)));
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
        if (idDestino.Equals(other.idDestino) && idOrigem.Equals(other.idOrigem) && distancia.Equals(other.distancia))
            return 0;

        return this.distancia - other.distancia;
    }
}

