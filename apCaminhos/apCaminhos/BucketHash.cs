using System;
using System.Collections;
using System.Collections.Generic;

class BucketHash
{
  private const int SIZE = 17;
  ArrayList[] data;
  public BucketHash()
  {
    data = new ArrayList[SIZE];
    for (int i = 0; i < SIZE; i++)
        data[i] = new ArrayList(1);
  }

  public int Hash(string s)
  {
    long tot = 0;
    char[] charray;
    charray = s.ToUpper().ToCharArray();
    for (int i = 0; i <= s.Length - 1; i++)
      tot += 37 * tot + (int)charray[i];
    tot = tot % data.GetUpperBound(0);
    if (tot < 0)
      tot += data.GetUpperBound(0);
    return (int)tot;
  }

  public void Insert(string item)
  {
    int hash_value = Hash(item);
    if (!data[hash_value].Contains(item))
       data[hash_value].Add(item);
  }
  public bool Remove(string item)
  {
    int hash_value = Hash(item);
    if (data[hash_value].Contains(item))
    {
      data[hash_value].Remove(item);
      return true;
    }
    return false;
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