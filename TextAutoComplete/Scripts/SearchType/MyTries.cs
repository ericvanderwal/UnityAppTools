using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace dgd
{
    public class MyTries : MonoBehaviour
    {
        public string textpath;

        // Start is called before the first frame update
        void Start()
        {
            Init();
        }

        private void Init()
        {
            var items = new List<string> {"armed", "armed", "jazz", "jaws"};
            var stream = new StreamReader(textpath);

            while (!stream.EndOfStream)
                items.Add(stream.ReadLine());

            var stopwatch = new Stopwatch();

            var trie = new Trie();
            var hashset = new HashSet<string>();
            const string s = "gau";

            stopwatch.Start();
            trie.InsertRange(items);
            stopwatch.Stop();
            Debug.Log("Trie insertion in ticks: " + stopwatch.ElapsedTicks);

            stopwatch.Reset();

            stopwatch.Start();
            for (int i = 0; i < items.Count; i++)
                hashset.Add(items[i]);
            stopwatch.Stop();
            Debug.Log("HashSet in ticks: " + stopwatch.ElapsedTicks);
            stopwatch.Reset();

            stopwatch.Start();
            var prefix = trie.Prefix(s);
            var foundT = prefix.Depth == s.Length && prefix.FindChildNode('$') != null;
            stopwatch.Stop();
            Debug.Log("Trie search in x ticks: " + stopwatch.ElapsedTicks + " found " + foundT);
            stopwatch.Reset();

            stopwatch.Start();
            var foundL = hashset.FirstOrDefault(str => str.StartsWith(s));
            stopwatch.Stop();
            Debug.Log("Hashset search in x ticks: " + stopwatch.ElapsedTicks + " found " + foundL);

//        trie.Delete("jazz");
//        Console.Read();
        }
    }
}

namespace dgd
{
    public class Node
    {
        public char Value { get; set; }
        public List<Node> Children { get; set; }
        public Node Parent { get; set; }
        public int Depth { get; set; }

        public Node(char value, int depth, Node parent)
        {
            Value = value;
            Children = new List<Node>();
            Depth = depth;
            Parent = parent;
        }

        public bool IsLeaf()
        {
            return Children.Count == 0;
        }

        public Node FindChildNode(char c)
        {
            foreach (var child in Children)
                if (child.Value == c)
                    return child;

            return null;
        }

        public void DeleteChildNode(char c)
        {
            for (var i = 0; i < Children.Count; i++)
                if (Children[i].Value == c)
                    Children.RemoveAt(i);
        }
    }
}

namespace dgd
{
    public class Trie
    {
        private readonly Node _root;

        public Trie()
        {
            _root = new Node('^', 0, null);
        }

        public Node Prefix(string s)
        {
            var currentNode = _root;
            var result = currentNode;

            foreach (var c in s)
            {
                currentNode = currentNode.FindChildNode(c);
                if (currentNode == null)
                    break;
                result = currentNode;
            }

            return result;
        }

        public bool Search(string s)
        {
            var prefix = Prefix(s);
            return prefix.Depth == s.Length && prefix.FindChildNode('$') != null;
        }

        public void InsertRange(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
                Insert(items[i]);
        }

        public void Insert(string s)
        {
            var commonPrefix = Prefix(s);
            var current = commonPrefix;

            for (var i = current.Depth; i < s.Length; i++)
            {
                var newNode = new Node(s[i], current.Depth + 1, current);
                current.Children.Add(newNode);
                current = newNode;
            }

            current.Children.Add(new Node('$', current.Depth + 1, current));
        }

        public void Delete(string s)
        {
            if (Search(s))
            {
                var node = Prefix(s).FindChildNode('$');

                while (node.IsLeaf())
                {
                    var parent = node.Parent;
                    parent.DeleteChildNode(node.Value);
                    node = parent;
                }
            }
        }
    }
}