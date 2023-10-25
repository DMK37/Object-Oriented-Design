using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.AccessControl;

namespace ObjectOrientedDesign
{
    public class BinaryTree<T> : IAbstractList<T>
    {
        internal Node Head;

        public class Node
        {
            public T Obj;
            public Node Parent;
            public Node Left;
            public Node Right;
            
            public Node(T obj, Node parent, Node left, Node right)
            {
                Obj = obj;
                Parent = parent;
                Left = left;
                Right = right;
            }
        }

        public void Insert(T element)
        {
            if (Equals(Head, null))
            {
                Head = new Node(element, null, null, null);
                return;
            }

            Node p = Head;
            Random rnd = new Random();
            while (!Equals(p.Left, null) && !Equals(p.Right, null))
            {
                int tmp = rnd.Next(0,2);
                p = tmp == 0 ? p.Left : p.Right;
            }

            if (Equals(p.Left, null))
                p.Left = new Node(element, p, null, null);
            else
            {
                p.Right = new Node(element, p, null, null);
            }
        }

        public void Remove(T element)
        {
            Stack<Node> stack = new Stack<Node>();
            if(Head != null)
                stack.Push(this.Head);
            while (stack.Count > 0)
            {
                Node p = stack.Pop();
                if (Equals(p.Obj, element))
                {
                    if (p.Left == null && p.Right == null)
                    {
                        if (p.Parent == null)
                            Head = null;
                        else
                        {
                            if (p.Parent.Left == p)
                                p.Parent.Left = null;
                            else
                            {
                                p.Parent.Right = null;
                            }
                        }

                        p.Parent = null;
                        break;
                    }
                    Random rnd = new Random();
                    Node t = p;
                    while (p.Right != null && p.Left != null)
                    {
                        int tmp = rnd.Next(0,2);
                        p = tmp == 0 ? p.Left : p.Right;
                    }

                    while (p.Left != null)
                        p = p.Left;
                    while (p.Right != null)
                        p = p.Right;
                    if (p.Parent.Left == p)
                        p.Parent.Left = null;
                    else
                    {
                        p.Parent.Right = null;
                    }
                    p.Left = t.Left;
                    p.Right = t.Right;
                    if (t.Parent == null)
                    {
                        
                        t.Left = null;
                        t.Right = null;
                        Head = p;
                    }
                    else
                    {
                        if (t.Parent.Left == t)
                            t.Parent.Left = p;
                        if (t.Parent.Right == t)
                            t.Parent.Right = p;
                    }
                    if (p.Left != null)
                        p.Left.Parent = p;
                    if (p.Right != null)
                        p.Right.Parent = p;
                    p.Parent = t.Parent;
                    break;
                }
                if(p.Left != null)
                    stack.Push(p.Left);
                if(p.Right != null)
                    stack.Push(p.Right);
            }
            
        }

        public IIterator<T> CreateIterator()
        {
            return new BinaryIterator<T>(this);
        }

        public IIterator<T> CreateRevIterator()
        {
            return new BinaryRevIterator<T>(this);
        }

        
    }

    public class BinaryIterator<T> : IIterator<T>
    {
        private BinaryTree<T> Tree;
        private BinaryTree<T>.Node current;
        private Stack<BinaryTree<T>.Node> st;
        
        public T First()
        {
            return Tree.Head.Obj;
        }

        public T Next()
        {
            var tmp = current;
            if(!Equals(current.Right,null))
                st.Push(current.Right);
            if(!Equals(current.Left,null))
                st.Push(current.Left);
            if (st.Count > 0)
                current = st.Pop();
            else
            {
                current = null;
            }

            return tmp.Obj;
        }

        public BinaryIterator(BinaryTree<T> tree)
        {
            Tree = tree;
            if (!Equals(tree, null))
                current = tree.Head;
            st = new Stack<BinaryTree<T>.Node>();
        }

        public bool HasNext => !Equals(current, null);
        public BinaryTree<T>.Node Current => current;
    }

    public class BinaryRevIterator<T> : IIterator<T>
    {
        private BinaryTree<T> Tree;
        private BinaryTree<T>.Node current;
        //private List<T> st;
        private int index = 0;
        private BinaryTree<T>.Node start;

        public T First()
        {
            return start.Obj;
        }

        public T Next()
        {
            var tmp = current;
            if (current.Parent == null)
            {
                current = null;
                return tmp.Obj;
            }

            if (current.Parent.Right == current)
            {
                current = current.Parent.Left != null ? leftRight(current.Parent.Left) : current.Parent;
            }
            else
            {
                current = current.Parent;
            }
            return tmp.Obj;
        }

        private BinaryTree<T>.Node leftRight(BinaryTree<T>.Node node)
        {
            BinaryTree<T>.Node p = node;
            while (p.Left != null || p.Right != null)
            {
                if (p.Right != null)
                    p = p.Right;
                else
                    p = p.Left;
            }

            return p;
        }

        public BinaryRevIterator(BinaryTree<T> tree)
        {
            Tree = tree;
            if (!Equals(tree, null))
                current = tree.Head;
            BinaryTree<T>.Node p = tree.Head;
            while (p.Right != null)
            {
                p = p.Right;
            }

            if (p.Left != null)
            {
                p = p.Left;
            }
            start = p;
            current = start;
        }

        public bool HasNext => !Equals(current, null);
        public BinaryTree<T>.Node Current => current;
    }
    
}