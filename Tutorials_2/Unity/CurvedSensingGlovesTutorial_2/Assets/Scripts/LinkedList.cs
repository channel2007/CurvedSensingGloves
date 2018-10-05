using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    abstract class LinkedList<T>
    {
        public int Count { get; protected set; }
        public Node<T> First { get; protected set; }
        public Node<T> Last { get; protected set; }
        abstract public void AddFirst(T value);
        abstract public void AddLast(T value);
        abstract public void AddBefore(Node<T> node, T value);
        abstract public void AddAfter(Node<T> node, T value);
        abstract public void RemoveFirst();
        abstract public void RemoveLast();
        abstract public void Remove(Node<T> node);
    }

    class Node<T>
    {
        public Node<T> Next { get; internal set; }
        public Node<T> Previous { get; internal set; }
        public T Value { get; internal set; }

        public Node(T value)
        {
            Value = value;
        }
    }
}