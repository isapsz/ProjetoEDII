using System;

namespace OrdenacoesSimples
{
    class CArray
    {
        private int[] arr;
        private int upper;
        private int numElements;

        public CArray(int size)
        {
            arr = new int[size];
            upper = size - 1;
            numElements = 0;
        }
        public void Insert(int item)
        {
            arr[numElements] = item;
            numElements++;
        }
        public void DisplayElements()
        {
            for (int i = 0; i <= upper; i++)
                Console.Write(arr[i] + " ");
        }
        public void Clear()
        {
            for (int i = 0; i <= upper; i++)
                arr[i] = 0;
        }
        public void BubbleSort()
        {
            int temp, linha = 0; // linha será usada para exibir as trocas de valores
            for (int outer = upper; outer >= 0; outer--)
            {
                for (int inner = 0; inner <= outer - 1; inner++)
                    if (arr[inner] > arr[inner + 1])
                    {
                        temp = arr[inner];
                        arr[inner] = arr[inner + 1];
                        arr[inner + 1] = temp;
                    }
            }
        }
        public void SelectionSort()
        {
            int min, temp, linha = 0; // linha será usada para exibir as trocas de valores
            for (int outer = 0; outer <= upper; outer++)
            {
                min = outer;
                for (int inner = outer + 1; inner <= upper; inner++)
                    if (arr[inner] < arr[min])
                        min = inner;
                if (outer != min)
                {
                    temp = arr[outer];
                    arr[outer] = arr[min];
                    arr[min] = temp;
                }
            }
        }
        public void InsertionSort()
        {
            int inner, temp, linha = 0;
            for (int outer = 1; outer < numElements; outer++)
            {
                temp = arr[outer];
                inner = outer;
                while (inner > 0 && arr[inner - 1] > temp)
                {
                    arr[inner] = arr[inner - 1];
                    inner -= 1;
                }
                arr[inner] = temp;
            }
        }
        public CArray Copiar()
        {
            CArray copia = new CArray(numElements);
            for (int indice = 0; indice < numElements; indice++)
                copia.Insert(arr[indice]);
            return copia;
        }
        public void ShellSort()
        {
            int inner, temp, linha = 0;
            int h = 1;
            while (h <= numElements / 3)
                h = h * 3 + 1;

            while (h > 0)
            {
                for (int outer = h; outer <= numElements - 1; outer++)
                {
                    temp = arr[outer];
                    inner = outer;
                    while ((inner > h - 1) && arr[inner - h] >= temp)
                    {
                        arr[inner] = arr[inner - h];
                        inner -= h;
                    }
                    arr[inner] = temp;
                }
                h = (h - 1) / 3;
            }
        }
        public void MergeSort()
        {
            int[] tempArray = new int[numElements];
            RecMergeSort(tempArray, 0, numElements - 1);
        }
        public void RecMergeSort(int[] tempArray, int esquerdo, int direito)
        {
            int linha = 0;
            if (esquerdo == direito)
                return;
            else
            {
                int meio = (int)(esquerdo + direito) / 2;
                RecMergeSort(tempArray, esquerdo, meio);
                RecMergeSort(tempArray, meio + 1, direito);
                Merge(tempArray, esquerdo, meio + 1, direito);
            }
        }
        public void Merge(int[] tempArray, int lowp, int highp, int ubound)
        {
            int lbound = lowp;
            int mid = highp - 1;
            int j = lowp;
            int n = (ubound - lbound) + 1;
            while ((lowp <= mid) && (highp <= ubound))
                if (arr[lowp] < arr[highp])
                    tempArray[j++] = arr[lowp++];
                else
                    tempArray[j++] = arr[highp++];
            while (lowp <= mid)
                tempArray[j++] = arr[lowp++];
            while (highp <= ubound)
                tempArray[j++] = arr[highp++];
            for (int i = 0; i <= n - 1; i++)
                arr[lbound + i] = tempArray[lbound + i];
        }
        public void QuickSort()
        {
            RecQSort(0, numElements - 1);
        }
        public void RecQSort(int inicio, int fim)
        {
            if (fim <= inicio)
                return;
            else
            {
                int pivot = arr[fim];
                int part = this.Partition(inicio, fim);
                RecQSort(inicio, part - 1);
                RecQSort(part + 1, fim);
            }
        }
        public int Partition(int inicio, int fim)
        {
            int pivotVal = arr[inicio];
            int oPrimeiro = inicio;
            bool okSide;
            inicio++;
            do
            {
                okSide = true;
                while (okSide) // 4
                    if (arr[inicio] > pivotVal)
                        okSide = false;
                    else
                    {
                        inicio++;
                        okSide = (inicio <= fim);
                    }
                okSide = (inicio <= fim);
                while (okSide) // 4
                    if (arr[fim] <= pivotVal)
                        okSide = false;
                    else
                    {
                        fim--;
                        okSide = (inicio <= fim);
                    }
                if (inicio < fim)
                {
                    Trocar(inicio, fim);
                    inicio++;
                    fim--;
                }
            } while (inicio <= fim);
            Trocar(oPrimeiro, fim);
            return fim;
        }
        public void Trocar(int item1, int item2)
        {
            int temp = arr[item1];
            arr[item1] = arr[item2];
            arr[item2] = temp;
        }
    }
}
