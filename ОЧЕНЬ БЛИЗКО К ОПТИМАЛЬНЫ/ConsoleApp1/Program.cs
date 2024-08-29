using System.Xml.Linq;

namespace P;
class Node<T>
{
    public T Data;
    public Node<T> Left, Right;

    public Node()
    {
        
    }

    public Node(T item)
    {
        Data = item;
        Left = Right = null;
    }
}

class Tree<T>
{
    private Node<T> root;

    public Tree()
    {
        root = null;
    }

    public Tree(Node<T> root)
    {
        this.root = root;
    }


    public void Print()
    {
        PrintTree(root);
    }
    static void PrintTree(Node<T> node, string indent = "", bool last = true)
    {
        if (node == null) return;

        Console.Write(indent);
        if (last)
        {
            Console.Write("└─");
            indent += "  ";
        }
        else
        {
            Console.Write("├─");
            indent += "| ";
        }

        if (node.Data != null)
            Console.WriteLine(node.Data.ToString());

        PrintTree(node.Left, indent, node.Right == null);
        PrintTree(node.Right, indent, true);
    }


    public static Tuple<double, Tree<P>> OptimalBST<P>(P[] keys, int[] weights)
    {
        int n = keys.Length;
        var cost = new int[n + 1, n + 1];
        var root = new Node<P>[n + 1, n + 1];

        for (int l = 2; l <= n; l++)
        {
            for (int i = 0; i <= n - l + 1; i++)
            {
                int j = i + l - 1;
                cost[i, j] = int.MaxValue;
                var ijWeight = weights[i..j].Sum();
                for (int k = i; k <= j; k++)    
                {
                    int c = ((k > i) ? cost[i, k - 1] : 0) +
                            ((k < j) ? cost[k + 1, j] : 0) +
                            ijWeight;

                    if (c < cost[i, j])
                    {
                        cost[i, j] = c;
                        root[i, j] = new(keys[k]);
                        root[i, j].Left = (k > i) ? root[i, k - 1] : null;
                        root[i, j].Right = (k < j) ? root[k + 1, j] : null;
                    }
                }
            }
        }

        return Tuple.Create((double)cost[0, n - 1] / weights.Sum(), new Tree<P>(root[0, n - 1]));
    }

    public static Tuple<double, Tree<P>> BuildAlmostOptimalTree<P>(IDictionary<P, int> values)
    {
        var sorted = values.OrderByDescending(kv => kv.Value).ToArray();
        var roots = new Node<P>[sorted.Length];
        for (int i = 0; i < sorted.Length; i++) 
            roots[i] = new Node<P>(sorted[i].Key);

        double sum = 0;
        for(int i = 0; i < sorted.Length; i++)
        {
            var level = (int)Math.Log2(i + 1) + 1;
            sum += level * sorted[i].Value;

            if (2 * i + 1 < sorted.Length)
                roots[i].Left = roots[2 * i + 1];
            if (2 * i + 2 < sorted.Length)
                roots[i].Right = roots[2 * i + 2];
        }
        return Tuple.Create(sum / values.Select(kv => kv.Value).Sum(), new Tree<P>(roots[0]));
    }

    public static Tuple<double, Tree<P>> BuildAlmostOptimalTree2<P>(IDictionary<P, int> values)
    {
        var sorted = values.OrderBy(kv => kv.Value).ToArray();
        double sum = 0;
        var level = 0;
        var result = RecAOT2(0, sorted.Length, sorted, ref sum, ref level);
        return Tuple.Create(sum / values.Select(kv => kv.Value).Sum(), new Tree<P>(result));
    }

    static Node<P> RecAOT2<P>(int start, int end, KeyValuePair<P, int>[] values, ref double sum, ref int level)
    {
        if (start >= end)
            return null;

        var totalSum = values[start..end].Select(kv => kv.Value).Sum();
        var partSum = 0;
        int i = start;
        for (i = start; i < end; i++)
        {
            partSum += values[i].Value;
            totalSum -= values[i].Value;
            if (partSum >= totalSum)
                break;
        }

        sum += level * values[i].Value;
        var curLevel = level;
        var node = new Node<P>(values[i].Key);
        level++;
        node.Left = RecAOT2(start, i, values, ref sum, ref level);

        level = curLevel;
        level++;
        node.Right = RecAOT2(i+1, end, values, ref sum, ref level);
        return node;
    }


}

class BinaryTree<T> : Tree<T> where T : IComparable<T>
{
    private Node<T> root;

    public BinaryTree()
    {
        root = null;
    }

    public BinaryTree(Node<T> root)
    {
        this.root = root;
    }

    public BinaryTree(IList<T> values)
    {
        BuildRec(values);
    }

    void BuildRec(IList<T> values)
    {
        var sortedValues = values.Order().ToArray();

        var center = sortedValues.Length / 2;
        var newNode = new Node<T>(sortedValues[center]);

        root = newNode;
        root.Left = BuildRec(sortedValues, 0, center);
        root.Right = BuildRec(sortedValues, center, values.Count);
    }

    static Node<T> BuildRec(IList<T> values, int start, int end)
    {
        var center = (start + end) / 2;
        var newNode = new Node<T>(values[center]);
        if ((start, end) == (0, 1))
        {
            return newNode;
        }
        if (end - start == 1)
        {
            return null;
        }
        newNode.Left = BuildRec(values, start, center);
        newNode.Right = BuildRec(values, center, end);
        return newNode;
    }

    public void Insert(T key)
    {
        root = InsertRec(root, key);
    }

    private Node<T> InsertRec(Node<T> root, T key)
    {
        if (root == null)
        {
            root = new Node<T>(key);
            return root;
        }

        if (key.CompareTo(root.Data) < 0)
        {
            root.Left = InsertRec(root.Left, key);
        }
        else if (key.CompareTo(root.Data) > 0)
        {
            root.Right = InsertRec(root.Right, key);
        }

        return root;
    }

    public void Delete(T key)
    {
        root = DeleteRec(root, key);
    }

    private Node<T> DeleteRec(Node<T> root, T key)
    {
        if (root == null)
        {
            return root;
        }

        if (key.CompareTo(root.Data) < 0)
        {
            root.Left = DeleteRec(root.Left, key);
        }
        else if (key.CompareTo(root.Data) > 0)
        {
            root.Right = DeleteRec(root.Right, key);
        }
        else
        {
            if (root.Left == null)
            {
                return root.Right;
            }
            else if (root.Right == null)
            {
                return root.Left;
            }

            root.Data = MinValue(root.Right);

            root.Right = DeleteRec(root.Right, root.Data);
        }

        return root;
    }

    private T MinValue(Node<T> root)
    {
        T minv = root.Data;
        while (root.Left != null)
        {
            minv = root.Left.Data;
            root = root.Left;
        }
        return minv;
    }

    public bool Search(T key)
    {
        return SearchRec(root, key);
    }

    private bool SearchRec(Node<T> root, T key)
    {
        if (root == null)
        {
            return false;
        }

        if (key.CompareTo(root.Data) == 0)
        {
            return true;
        }

        if (key.CompareTo(root.Data) < 0)
        {
            return SearchRec(root.Left, key);
        }

        return SearchRec(root.Right, key);
    }

    public T FindMin()
    {
        Node<T> current = root;

        while (current.Left != null)
        {
            current = current.Left;
        }

        return current.Data;
    }

    public T FindMax()
    {
        Node<T> current = root;

        while (current.Right != null)
        {
            current = current.Right;
        }

        return current.Data;
    }



}


class Program
{

    static void Test(string[] keys, int[] priotities)
    {
        int pad = 3;

        Console.WriteLine(keys.Aggregate("", (a, b) => a.ToString().PadLeft(pad) + " " + b.ToString().PadLeft(pad)));
        Console.WriteLine(priotities.Aggregate("", (a, b) => a.ToString().PadLeft(pad) + " " + b.ToString().PadLeft(pad)));

        var optimalNode = BinaryTree<string>.OptimalBST(keys, priotities);
        var optimalTree = optimalNode.Item2;
        optimalTree.Print();
        Console.WriteLine(optimalNode.Item1);

        var almostOptimal = Tree<string>.BuildAlmostOptimalTree
            (keys.Zip(priotities).ToDictionary());

        var almostPptimalTree = almostOptimal.Item2;
        almostPptimalTree.Print();
        Console.WriteLine(almostOptimal.Item1);


        var almostOptimal2 = Tree<string>.BuildAlmostOptimalTree2
            (keys.Zip(priotities).ToDictionary());

        var almostPptimalTree2 = almostOptimal2.Item2;
        almostPptimalTree2.Print();
        Console.WriteLine(almostOptimal2.Item1);

    }
    public static void Main()
    {
        var r = new Random();
        var n = 100;
        string[] exKeys = new string[n];
        int[] exPriorities = new int[n];
        for (int i = 0; i < n; i++)
        {
            exKeys[i] = (Convert.ToChar('A' + i)).ToString();
            exPriorities[i] = r.Next(0, n);
        }

        string[] keys = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10"];
        int[] priotities = [5, 20, 5, 10, 5, 10, 10, 5, 5, 25];

        Test(exKeys, exPriorities);

    }
}