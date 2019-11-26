using System;
using System.Collections;
using System.Collections.Generic;

class BucketHash<T> where T : IBucketHash, IComparable<T>
{
    private const int SIZE = 17;
    ListaSimples<T>[] data;
    public BucketHash()
    {
        data = new ListaSimples<T>[SIZE];
        for (int i = 0; i < SIZE; i++)
            data[i] = new ListaSimples<T>();
    }

    public void Insert(T item)
    {
        int hash_value = item.HashCode();
        if (!data[hash_value].ExisteDado(item))
            data[hash_value].InserirEmOrdem(item);
    }

    public bool Remove(T item)
    {
        int hash_value = item.HashCode();
        if (data[hash_value].ExisteDado(item))
        {
            data[hash_value].Remover(item);
            return true;
        }
        return false;
    }
    
    public T this[T procurado]
    {
        get {
            if(data[procurado.HashCode()].ExisteDado(procurado))
              return procurado;

            return default(T);
        }
    }
}