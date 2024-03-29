﻿using System;
using System.IO;
using System.Runtime.Serialization;
//Amabile Pietrobon Ferreira - 18198
//Isabela Paulino de Souza - 18189
//Gustavo Ferreira Gitzel - 18194

public class Cidade : IComparable<Cidade>, IGravarEmArquivo
{
    /*
     Atributos inteiros constantes que armazenam os inicios e tamanhos de cada um dos atributos da classe quando estes
     estiverem em um arquivo texto formatado corretamente.
    */
    const int inicioId = 0,
              tamanhoId = 2,
              inicioNome = inicioId + tamanhoId,
              tamanhoNome = 16,
              inicioCoordenadaX = inicioNome + tamanhoNome,
              tamanhoCoordenadaX = 5,
              inicioCoordenadaY = inicioCoordenadaX + tamanhoCoordenadaX;

    /*
    Atributo inteiro que representa o id da cidade.
    */
    int id;

    /*
     Atributos reais que representam as coordenadas da cidade
    */
    float coordenadaX, coordenadaY;

    /*
     Atributo string que representam o nome da cidade.
    */
    string nome;

    /*
     Propriedade que retorna e altera o valor do atributo inteiro que representa o id da cidade.
    */
    public int Id
    {
        get => id;
        set
        {
            if (value >= 0)
                id = value;
        }
    }

    /*
     Propriedade que retorna e altera o valor do atributo que representa o nome da cidade.
    */
    public string Nome
    {
        get => nome;
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                nome = value;
        }
    }

    /*
      Propriedade que retorna e altera o valordo atributo inteiro que representa a coordenada horizontal da cidade.
    */
    public float CoordenadaX
    {
        get => coordenadaX;
        set
        {
            if (value >= 0)
                coordenadaX = value;
        }
    }

    /*
      Propriedade que retorna e altera o valordo atributo inteiro que representa a coordenada vertical da cidade.
    */
    public float CoordenadaY
    {
        get => coordenadaY;
        set
        {
            if (value >= 0)
                coordenadaY = value;
        }
    }

    /*
      Sobrecarga do construtor vazio que não recebe parâmetros e inicia os atributos com valores padrões.
    */
    public Cidade() 
    {
        id =  -1;
        coordenadaX = coordenadaY = -1f;
        nome = null;
    }

    /*
      Sobrecarga do construtor que não recebe como parâmetro todos os atributos da classe e os instância com base nos parâmetros.
      @params os atributos inteiros id da cidade, coordenadas de onde está localizada e uma string com o nome da cidade.
    */
    public Cidade(int id, string nome, float coordenadaX, float coordenadaY)
    {
        Id = id;
        Nome = nome;
        CoordenadaX = coordenadaX;
        CoordenadaY = coordenadaY;
    }

    /*
     Sobrecarga do construtor que não recebe como parâmetro só o atributo id e inicia os outros atributos com valores padrões.
     @params os atributos inteiros id da cidade.
    */
    public Cidade(int id)
    {
        Id = id;
        coordenadaX = coordenadaY = -1;
        nome = null;
    }


    public Cidade(string nome)
    {
        coordenadaX = coordenadaY = id = -1;
        this.nome = nome;
    }

    /*
     Método que lê uma linha de um StreamReader de um arquivo que contém as cidades e retorna uma Cidade com base na linha
     lida daquele arquivo.
     @params o StreamReader do arquivo que está sendo lido.
     @return uma Cidade com as informações contidas naquelas linha lida do arquivo.
    */
    public static Cidade LerRegistro(StreamReader arq)
    {
        Cidade ret = null;
        try
        {
            if (!arq.EndOfStream)
            {
                ret = new Cidade();
                string linha = arq.ReadLine();
                linha = linha.Replace(',', '.');
                ret.Id = int.Parse(linha.Substring(inicioId, tamanhoId));
                ret.Nome = linha.Substring(inicioNome, tamanhoNome).Trim().ToUpper();
                ret.CoordenadaX = float.Parse(linha.Substring(inicioCoordenadaX, tamanhoCoordenadaX));
                ret.CoordenadaY = float.Parse(linha.Substring(inicioCoordenadaY));
            }
        }
        catch (Exception erro)
        {
            throw new System.IO.IOException(erro.Message);
        }
        return ret;
    }

    /*
      Método que compara duas cidades com base no id.
      @params outro Caminho que será usado para comparar.
      @return um int que é a diferença entre as duas distâncias e se for a mesma retorna 0.
    */
    public int CompareTo(Cidade outra)
    {
        //if((this.id != -1 && outra.id != -1))
       //     return this.id - outra.Id;

        return this.nome.CompareTo(outra.nome);
    }

    /*
      Método que gera uma representação textual de uma cidade.
      @return uma string com a representação textual da classe.
    */
    public override string ToString()
    {
        return $"{id:00}\n\n{nome}";
    }

    /*
     Método que gera uma representação textual para arquivos de uma cidade.
     @return uma string com a representação da classe.
   */
    public string ParaArquivo()
    {
        return $"{id.ToString().PadLeft(2)}{nome.PadRight(16)}{coordenadaX.ToString("0.000")} {coordenadaY.ToString("0.000")}";
    }
}