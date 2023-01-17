using System;
using System.Collections;
using System.Collections.Generic;

public class DynamicMatrix<T> : IEnumerable<T>
{
    public readonly T[][] rows;
    public readonly T[][] cols;

    private readonly T[] items;

    public int Size { get; private set; }

    public T[] MainDiagonal
    {
        get
        {
            var result = new T[Size];

            for (int i = 0; i < Size; i++)
                result[i] = rows[i][i];

            return result;
        }
    }

    public T[] AntiDiagonal
    {
        get
        {
            var result = new T[Size];

            for (int i = 0; i < Size; i++)
                result[i] = rows[i][Size - i - 1];

            return result;
        }
    }

    public DynamicMatrix(T[] items)
    {
        this.items = items;
        Size = (int)Math.Sqrt(items.Length);

        rows = new T[Size][];
        cols = new T[Size][];

        for (int i = 0; i < Size; i++)
        {
            var row = new T[Size];

            for (int j = 0; j < Size; j++)
                row[j] = items[i * Size + j];

            rows[i] = row;
        }

        for (int i = 0; i < Size; i++)
        {
            var col = new T[Size];

            for (int j = 0; j < Size; j++)
                col[j] = rows[j][i];

            cols[i] = col;
        }
    }

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)items).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();
}