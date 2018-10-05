using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    class SinglyLinkedList<T> : LinkedList<T>
    {
        public override void AddFirst(T value)
        {
            Node<T> node = new Node<T>(value);
            if (Count == 0)
                Last = node;
            else
                node.Next = First;
            First = node;
            ++Count;
        }

        public override void AddLast(T value)
        {
            Node<T> node = new Node<T>(value);
            if (Count == 0)
                First = node;
            else
                Last.Next = node;
            Last = node;
            ++Count;
        }

        public override void AddBefore(Node<T> node, T value)
        {
            Node<T> newNode = new Node<T>(value);
            if (node == First)
            {
                First = newNode;
            }
            else
            {
                Node<T> preNode = FindPreviousNode(node);
                preNode.Next = newNode;
            }
            newNode.Next = node;
            ++Count;
        }

        public override void AddAfter(Node<T> node, T value)
        {
            Node<T> newNode = new Node<T>(value);
            newNode.Next = node.Next;
            node.Next = newNode;
            if (node == Last)
            {
                Last = newNode;
            }
            ++Count;
        }

        public override void RemoveFirst()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException();
            else if (Count == 1)
            {
                First = null;
                Last = null;
            }
            else
            {
                Node<T> node = First.Next;
                First.Next = null;
                First = node;
            }
            --Count;
        }

        public override void RemoveLast()
        {
            if (Count == 0)
                throw new IndexOutOfRangeException();
            else if (Count == 1)
            {
                First = null;
                Last = null;
            }
            else
            {
                Node<T> node = FindPreviousNode(Last);
                node.Next = null;
                Last = node;
            }
            --Count;
        }

        public override void Remove(Node<T> node)
        {
            if (node == First)
                RemoveFirst();
            else if (node == Last)
                RemoveLast();
            else
            {
                Node<T> preNode = FindPreviousNode(node);
                if (preNode == null)
                    throw new IndexOutOfRangeException();
                preNode.Next = node.Next;
                node.Next = null;
                --Count;
            }
        }

        private Node<T> FindPreviousNode(Node<T> node)
        {
            Node<T> preNode = First;
            while (preNode != null)
            {
                if (node == preNode.Next)
                    break;
                preNode = preNode.Next;
            }
            return preNode;
        }
    }
}