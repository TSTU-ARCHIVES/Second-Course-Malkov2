namespace Tree;

public class TreeNode<T>(T value)
{
    public T Value { get; set; } = value;

    public TreeNode<T> Child { get; set; }

    public TreeNode<T> Neighbour { get; set; }

    public IEnumerable<TreeNode<T>> GetSibilings()
    {
        var cur = Neighbour;
        while(cur != null)
        {
            yield return cur;
            cur = cur.Neighbour;
        }
        yield break;
    }

    public IEnumerable<TreeNode<T>> GetChilds()
    {
        var cur = Child;
        while (cur != null)
        {
            yield return cur;
            cur = cur.Neighbour;
        }
        yield break;
    }

    public void AddChild(TreeNode<T> child)
    {
        if (Child == null)
        {
            Child = child;
            return;
        }
        var cur = Child;
        while (cur.Neighbour != null)
        {
            cur = cur.Neighbour;
        }
        cur.Neighbour = child;
    }

}
