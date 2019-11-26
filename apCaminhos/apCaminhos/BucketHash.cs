using System;
using System.Collections;
using System.Collections.Generic;

class BucketHash<T> where T : IBucketHash
{
    private const int SIZE = 17;
    ArrayList[] data;
    public BucketHash()
    {
        data = new ArrayList[SIZE];
        for (int i = 0; i < SIZE; i++)
            data[i] = new ArrayList(1);
    }

    public void Insert(T item)
    {
        int hash_value = item.HashCode();
        if (!data[hash_value].Contains(item))
            data[hash_value].Add(item);
    }

    public bool Remove(T item)
    {
        int hash_value = item.HashCode();
        if (data[hash_value].Contains(item))
        {
            data[hash_value].Remove(item);
            return true;
        }
        return false;
    }
    
    public T this[T procurado]
    {
        get {
            ArrayList array = data[procurado.HashCode()];
            foreach (T t in array)
                if (t.Equals(procurado))
                    return t;

            return default(T);
        }
    }

    public void Exibir()
    {
        for (int i = 0; i <= data.GetUpperBound(0); i++)
        {
            if (data[i].Count > 0)
            {
                Console.Write($"{i,3} : ");
                foreach (string chave in data[i])
                    Console.Write(" | " + chave);
                Console.WriteLine();
            }
        }
        Console.ReadKey();
    }
}