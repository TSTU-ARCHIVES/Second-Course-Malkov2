using ClassLib;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

namespace ConsoleTest;

public static class Program
{
    static void Test1()
    {
        var table = new UniHashTable<string, string>();
        var count = 20;
        for (int i = 0; i < count; i++)
        {
            table.Add($"key{i}", $"value{i}");
        }

        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(table[$"key{i}"]);
        }

        MyList<MyKeyValuePair<string, string>> kvs = [];
        for (int i = 0; i < count; i++)
        {
            kvs.Add(new($"key{i}", $"value{i}"));
        }

        var perfectTable = new PerfectHashTable<string, string>(kvs);
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(perfectTable.Get($"key{i}"));
        }

        foreach (var kv in perfectTable.GetKeyValuePairs())
            Console.WriteLine(kv);
        Console.ReadKey();
    }


    static double StringIntPerfectTableTest(int n)
    {
        var values = Enumerable.Range(0, n).Select(x => "key" + x.ToString());
        var keys = Enumerable.Range(0, n).Select(x => (x * x) % 147);
        var kvs = values.Zip(keys).Select(kv => new MyKeyValuePair<string, int>(kv.First, kv.Second));
        var perfectTable
            = new PerfectHashTable<string, int>(kvs);

        var sw = new Stopwatch();
        sw.Start();
        foreach (var val in values)
        {
            var t = perfectTable.Get(val);
        }

        sw.Stop();
        return (double)sw.ElapsedTicks / values.Count();
    }

    static double StringIntUniTableTest(int n)
    {
        var values = Enumerable.Range(0, n).Select(x => "key" + x.ToString());
        var keys = Enumerable.Range(0, n).Select(x => (x * x) % 147);
        var kvs = values.Zip(keys).Select(kv => new MyKeyValuePair<string, int>(kv.First, kv.Second));

        var sw = new Stopwatch();
        var uniTable = new UniHashTable<string, int>();
        foreach (var (key, val) in kvs)
            uniTable.Add(key, val);

        sw.Start();
        foreach (var val in values)
        {
            var t = uniTable.Get(val);
        }

        sw.Stop();
        return (double)sw.ElapsedTicks / values.Count();
    }
    public static void Main()
    {

        int[] testCounts = [10, 100, 1000, 5000, 10000, 25000, 50000, 
            100_000, 250_000, 500_000];
        int excessIterations = 5;
        foreach(var amount in testCounts)
        {
            Console.WriteLine("выборка: " + amount + " элементов");
            var medianPefect = StringIntPerfectTableTest(amount);
            var medianUni = StringIntUniTableTest(amount);
            for(int i = 0; i < excessIterations; i++)
            {
                medianPefect = (medianPefect + StringIntPerfectTableTest(amount)) / 2;
                medianUni = (medianUni + StringIntUniTableTest(amount)) / 2;
            }
            Console.WriteLine("универсальная таблица: " + medianUni);
            Console.WriteLine("совершенная таблица: " + medianPefect);
            Console.WriteLine("_________________________________");
        }

        Console.ReadKey();

    }
}