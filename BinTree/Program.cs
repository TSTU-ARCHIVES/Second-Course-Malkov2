class Node<T> 
{
    public T data;
    public Node<T> left, right;

    public Node(T item)
    {
        data = item;
        left = right = null;
    }
}

class BinaryTree<T> where T : IComparable<T>
{
    private Node<T> root;

    public BinaryTree()
    {
        root = null;
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

        if (key.CompareTo(root.data) < 0)
        {
            root.left = InsertRec(root.left, key);
        }
        else if (key.CompareTo(root.data) > 0)
        {
            root.right = InsertRec(root.right, key);
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

        if (key.CompareTo(root.data) < 0)
        {
            root.left = DeleteRec(root.left, key);
        }
        else if (key.CompareTo(root.data) > 0)
        {
            root.right = DeleteRec(root.right, key);
        }
        else
        {
            if (root.left == null)
            {
                return root.right;
            }
            else if (root.right == null)
            {
                return root.left;
            }

            root.data = MinValue(root.right);

            root.right = DeleteRec(root.right, root.data);
        }

        return root;
    }

    private T MinValue(Node<T> root)
    {
        T minv = root.data;
        while (root.left != null)
        {
            minv = root.left.data;
            root = root.left;
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

        if (key.CompareTo(root.data) == 0)
        {
            return true;
        }

        if (key.CompareTo(root.data) < 0)
        {
            return SearchRec(root.left, key);
        }

        return SearchRec(root.right, key);
    }

    public T FindMin()
    {
        Node<T> current = root;

        while (current.left != null)
        {
            current = current.left;
        }

        return current.data;
    }s

    public T FindMax()
    {
        Node<T> current = root;

        while (current.right != null)
        {
            current = current.right;
        }

        return current.data;
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

        Console.WriteLine(node.data.ToString());

        PrintTree(node.left, indent, node.right == null);
        PrintTree(node.right, indent, true);
    }


}

class Program
{
    static void Main()
    {
        var intTree = new BinaryTree<int>();

        intTree.Insert(50);
        intTree.Insert(30);
        intTree.Insert(20);
        intTree.Insert(40);
        intTree.Insert(70);
        intTree.Insert(60);
        intTree.Insert(80);

        Console.WriteLine("Min value: " + intTree.FindMin());
        Console.WriteLine("Max value: " + intTree.FindMax());

        intTree.Delete(20);

        intTree.Print();

        if (intTree.Search(20))
            Console.WriteLine("Element 20 found");
        else
            Console.WriteLine("Element 20 not found");

        if (intTree.Search(30))
            Console.WriteLine("Element 30 found");
        else
            Console.WriteLine("Element 30 not found");


        var stringTree = new BinaryTree<string>();

        stringTree.Insert("apple");
        stringTree.Insert("banana");
        stringTree.Insert("orange");

        Console.WriteLine("Min value: " + stringTree.FindMin());
        Console.WriteLine("Max value: " + stringTree.FindMax());

        stringTree.Delete("banana");

        if (stringTree.Search("banana"))
            Console.WriteLine("Element 'banana' found");
        else
            Console.WriteLine("Element 'banana' not found");

        if (stringTree.Search("apple"))
            Console.WriteLine("Element 'apple' found");
        else
            Console.WriteLine("Element 'apple' not found");
    }
}