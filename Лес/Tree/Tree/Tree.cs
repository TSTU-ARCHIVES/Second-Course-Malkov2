namespace Tree;

public abstract class Tree<T>(TreeNode<T> root)
{
    public TreeNode<T> Root { get; init; } = root;

    public abstract void InsertValue(T value);
    public abstract void InStraightOrderTraverse(Action<TreeNode<T>> action);
    public abstract void InInverseOrderTraverse(Action<TreeNode<T>> action);
    public abstract void InSymmetricOrderTraverse(Action<TreeNode<T>> action);
}
