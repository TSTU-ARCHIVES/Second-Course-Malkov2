using System.Xml.Linq;

namespace Tree;

public class NotBinaryTree<T>(TreeNode<T> root) : Tree<T>(root)
{
    public override void InInverseOrderTraverse(Action<TreeNode<T>> action) =>
        InInverseOrderTraverse(action, Root);
    static void InInverseOrderTraverse(Action<TreeNode<T>> action, TreeNode<T> node)
    {
        foreach (var child in node.GetChilds())
        {
            InInverseOrderTraverse(action, child);
            action(child);
        }
        action(node);
    }
    public void InInverseOrderTraverseStack(Action<TreeNode<T>> action)
    {
        var stack = new Stack<TreeNode<T>>();
        var handled = new Dictionary<TreeNode<T>, bool>();

        InStraightOrderTraverseStack(node => handled.Add(node, false));

        stack.Push(Root);
        while (stack.Count > 0)
        {
            var node = stack.Peek();
            var flag = true;
            if (!handled[node])
                foreach (var child in node.GetChilds().Reverse())
                {
                    stack.Push(child);
                    handled[node] = true;
                    flag = false;
                }
            if (flag)
            {
                action(node);
                stack.Pop();
            }
        }
    }

    public override void InsertValue(T value)
    {

    }

    public void InStraightOrderTraverseStack(Action<TreeNode<T>> action)
    {
        var stack = new Stack<TreeNode<T>>();
        stack.Push(Root);
        while(stack.Count > 0)
        {
            var node = stack.Pop();
            action(node);
            foreach (var child in node.GetChilds().Reverse())
            {
                stack.Push(child);
            }
        }
    }

    public override void InStraightOrderTraverse(Action<TreeNode<T>> action) =>
        NotBinaryTree<T>.InStraightOrderTraverse(action, Root);

    static void InStraightOrderTraverse(Action<TreeNode<T>> action, TreeNode<T> node)
    {
        action(node);
        foreach (var child in node.GetChilds())
        {
            action(child);
            InStraightOrderTraverse(action, child);
        }
    }

    public override void InSymmetricOrderTraverse(Action<TreeNode<T>> action) =>
        InSymmetricOrderTraverse(action, Root);

    static void InSymmetricOrderTraverse(Action<TreeNode<T>> action, TreeNode<T> node)
    {
        if (node.Child != null)
        {
            action(node.Child);
            action(node);
            foreach (var nextChild in node.Child.GetSibilings())
            {
                InSymmetricOrderTraverse(action, nextChild);
                action(nextChild);
            }
        } else
        {
            action(node);
        }

    }

    public void InSymmetricOrderTraverseStack(Action<TreeNode<T>> action)
    {
        var frameStack = new Stack<TreeNode<T>>();
        var handled = new Dictionary<TreeNode<T>, bool>();
        InStraightOrderTraverseStack(node => handled.Add(node, true));

        frameStack.Push(Root);
        while(frameStack.Count > 0)
        {
            var node = frameStack.Peek(); 

            if (node.Child != null)
            {
                action(node.Child);
                action(node);
                bool flag = true;
                if (handled[node])
                    foreach (var nextChild in node.Child.GetSibilings().Reverse())
                    {
                        frameStack.Push(nextChild);
                        flag = false;
                        handled[node] = false;
                    }
                if (flag)
                    action(frameStack.Pop());
            }
            else
            {
                action(node);
                frameStack.Pop();
            }
        }
    }
}