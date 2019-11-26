using System;
using System.Collections;
using System.Collections.Generic;

class BucketHash
{
    private const int SIZE = 103;
    ListaSimples<Cidade>[] data;
    public BucketHash()
    {
        data = new ListaSimples<Cidade>[SIZE];
        for (int i = 0; i < SIZE; i++)
            data[i] = new ListaSimples<Cidade>();
    }
    protected int Hash(Cidade c)
    {
        long tot = 0;
        char[] charray;
        charray =c.Nome.ToUpper().ToCharArray();
        for (int i = 0; i <= c.Nome.Length - 1; i++)
            tot += 37 * tot + (int)charray[i];
        tot = tot % data.GetUpperBound(0);
        if (tot < 0)
            tot += data.GetUpperBound(0);
        return (int)tot;
    }

    public void Insert(Cidade item)
    {
        int hash_value = Hash(item);
        
        if (!data[hash_value].ExisteDado(item))
            data[hash_value].InserirEmOrdem(item);
    }

    public bool Remove(Cidade item)
    {
        int hash_value = Hash(item);
        if (data[hash_value].ExisteDado(item))
        {
            data[hash_value].Remover(item);
            return true;
        }
        return false;
    }
    
    public Cidade this[Cidade procurado]
    {
        get {
            if(data[Hash(procurado)].ExisteDado(procurado))
              return procurado;

            return default(Cidade);
        }
    }
}