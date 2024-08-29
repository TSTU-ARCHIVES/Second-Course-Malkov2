using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace P;
public static class Practise
{
    public static int RecursiveSum(this int[] array)
        => array.RecursiveSumHelper(array.Length - 1);
    private static int RecursiveSumHelper(this int[] array, int index)
        => index > 0 ? array[index] + RecursiveSumHelper(array, index - 1) : array[index];
    
    public static int StackSum(this int[] array)
    {
        var stack = new Stack<int>();
        stack.Push(array[0]);
        for (var i = 1; i < array.Length; i++)
        {
            stack.Push(stack.Peek() + array[i]);
        }
        return stack.Pop();
    }

    public static IEnumerable<int> RecursiveFibbonaci(int until)
    {
        return Enumerable.Range(0, until).Select(RecursiveFibbonaciHelper);
    }

    private static int RecursiveFibbonaciHelper(int iteration)
    {
        if (iteration == 0 || iteration == 1)
        {
            return 1;
        }
        else
        {
            return RecursiveFibbonaciHelper(iteration - 1) + RecursiveFibbonaciHelper(iteration - 2);
        }
    }

    public static Stack<int> StackFibbonaci(int until)
    {
        int a = 1, b = 1, i = 2;

        var stack = new Stack<int>();
        if (until > 0)
            stack.Push(a);
        if (until > 1)
            stack.Push(b);
        while (i < until)
        {
            var last = stack.Peek();
            stack.Pop();
            var predLast = stack.Peek();
            stack.Push(last);
            stack.Push(last + predLast);
            i++;
        }
        return stack;
    }

    public static void RecursiveFastSort<T>(this T[] arr) 
        where T: IComparable =>
        arr.RecurvieFastSortHelper(0, arr.Length - 1);
    
    private static void RecurvieFastSortHelper<T>(this T[] arr, int start, int end)
        where T:IComparable
    {
        int i = start, j = end;
        var pivot = arr[(start + end) / 2];
        while (i <= j)
        {
            while (arr[i].CompareTo(pivot) < 0)
            {
                i++;
            }

            while (arr[j].CompareTo(pivot) > 0)
            {
                j--;
            }
            if (i <= j)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]); 
                i++;
                j--;
            }
        }

        if (start < j)
        {
            Console.WriteLine("Recursive sort start j: " + start + " " + j);
            arr.RecurvieFastSortHelper(start, j);
        }
        if (i < end)
        {
            Console.WriteLine("Recursive sort i end: " + i + " " + end);
            arr.RecurvieFastSortHelper(i, end);
        }

    }

    public static void StackFastSort<T>(this T[] arr)
        where T : IComparable
    {

        var frameStack = new Stack<(int start, int end)>();
        frameStack.Push((0, arr.Length - 1));
        do
        {
            var (start, end) = frameStack.Pop();
            var (i,j) = arr.FastSortPartiton(start, end);
            if (start < j)
            {
                Console.WriteLine("Stack sort start j: " + i + " " + end);
                if (i < end)
                {
                    frameStack.Push((i, end));
                }
                frameStack.Push((start, j));
                continue;
            }
            if (i < end)
            {
                Console.WriteLine("Stack sort i end: " + i + " " + end);
                frameStack.Push((i, end));
                continue;
            }
            
        } while (frameStack.Count > 0);

        
    }

    private static (int, int) FastSortPartiton<T>(this T[] arr, int start, int end)
        where T:IComparable
    {
        int i = start, j = end;
        var pivot = arr[(start + end) / 2];
        while (i <= j)
        {
            while (arr[i].CompareTo(pivot) < 0)
            {
                i++;
            }

            while (arr[j].CompareTo(pivot) > 0)
            {
                j--;
            }
            if (i <= j)
            {
                (arr[i], arr[j]) = (arr[j], arr[i]);
                i++;
                j--;
            }
        }
        return (i, j);
    }


}
public class Program
{
    public static void Main(string[] Args)
    {
        int[] array = [354, 1, 2, 3, 4, 5, -10, 5, 5, 18, 23];

        var fibAmount = 10;
        Console.WriteLine(array.RecursiveSum());
        Console.WriteLine(array.StackSum());
        Console.WriteLine(Practise.StackFibbonaci(fibAmount).Aggregate("", (a,b) => a + " " + b));
        Console.WriteLine(Practise.RecursiveFibbonaci(fibAmount).Aggregate("", (a, b) => a + " " + b));
        Console.WriteLine(new string('_', 21));
        Console.WriteLine(array.Aggregate("", (a, b) => a + " " + b));
        array.RecursiveFastSort();
        Console.WriteLine(array.Aggregate("", (a, b) => a + " " + b));
        array = [354, 1, 2, 3, 4, 5, -10, 5, 5, 18, 23];
        array.StackFastSort();
        Console.WriteLine(array.Aggregate("", (a, b) => a + " " + b));


    }
}

