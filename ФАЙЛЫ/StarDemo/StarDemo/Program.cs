using System;
using System.Collections.Generic;
using System.Linq;

public class Node : IComparable<Node>
{
    public int Frequency { get; set; }
    public char Value { get; set; }
    public Node Left { get; set; }
    public Node Right { get; set; }

    public int CompareTo(Node other)
    {
        return Frequency.CompareTo(other.Frequency);
    }
}

public class Huffman
{
    public static Node BuildHuffmanTree(Dictionary<char, int> freqDict)
    {
        var heap = new SortedSet<Node>(freqDict.Select(kv => new Node { Frequency = kv.Value, Value = kv.Key }));

        while (heap.Count > 1)
        {
            var left = heap.Min;
            heap.Remove(left);

            var right = heap.Min;
            heap.Remove(right);

            var merged = new Node { Frequency = left.Frequency + right.Frequency };
            merged.Left = left;
            merged.Right = right;

            heap.Add(merged);
        }

        return heap.Min;
    }

    public static int CalculateBitsToSend(List<int> fileSizes)
    {
        int totalBits = 0;

        for (int i = 1; i < fileSizes.Count; i++)
        {
            var freqDict = new Dictionary<char, int> { { '0', fileSizes[i - 1] }, { '1', fileSizes[i] } };
            var root = BuildHuffmanTree(freqDict);
            totalBits += root.Frequency;
        }

        return totalBits;
    }

    public static void Main()
    {
        var fileSizes = new List<int> { 5, 7, 3 }; // Размеры файлов в битах
        var minBitsToSend = CalculateBitsToSend(fileSizes);
        Console.WriteLine($"Минимальное число битов для передачи файлов: {minBitsToSend}");
    }
}
