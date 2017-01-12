using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static Dictionary<int,Node> nodeList = new Dictionary<int, Node>();

    static void Main(string[] args)
    {
        
        int n = int.Parse(Console.ReadLine()); // the number of adjacency relations
        Console.Error.WriteLine("{0}", n);
        
        for (int i = 0; i < n; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            int xi = int.Parse(inputs[0]); // the ID of a person which is adjacent to yi
            int yi = int.Parse(inputs[1]); // the ID of a person which is adjacent to xi
            Console.Error.WriteLine("{0} {1}", xi, yi);

            CreateNodeAndLink(xi, yi, true);
        }

        bool cleaningCompleted = false;
        int parentDepth = 0;
        var saveNode = new Dictionary<int, Dictionary<int, int>>();
        while (!cleaningCompleted)
        {
            if(CleanTree() == 0)
            {
                cleaningCompleted = true;   
            }
            else
            {
                var depthNode = new Dictionary<int, int>();

                // Unlink & Save
                var idRoots = new List<int>();
                foreach (var node in nodeList.Values)
                {
                    if (node.Links.Count == 1)
                    {
                        idRoots.Add(node.Id);
                    }
                }

                foreach (var idRoot in idRoots)
                {
                    var node = nodeList[idRoot];
                    depthNode.Add(node.Id, node.Links[0].Id);
                    nodeList.Remove(node.Id);
                    node.RemoveLink();
                }

                saveNode.Add(parentDepth, depthNode);
                parentDepth++;
            }
        }

        for(int depth = parentDepth -1; depth >= 0; depth--)
        {
            Console.Error.WriteLine(depth);
            // link & Add if possible
            var depthNodes = saveNode[depth];
            foreach (var node in depthNodes)
            {
                CreateNodeAndLink(node.Key, node.Value, true);
            }
        }

        var max = 0;
        foreach(var node in nodeList.Values)
        {
            if (node.Links.Count == 1)
            {
                var count = Node.CountDepth(node, node);
                if (max < count)
                {
                    max = count;
                }
            }
        }
        Console.WriteLine((int)Math.Ceiling((double)max/2));
    }

    public static void CreateNodeAndLink(int xi, int yi, bool createSecondNode)
    {
        var firstNode = nodeList.ContainsKey(xi) ? nodeList[xi] : null;
        var secondNode = nodeList.ContainsKey(yi) ? nodeList[yi] : null;

        if (!createSecondNode && secondNode == null)
        {
            return;
        }

        if (firstNode == null)
        {
            firstNode = new Node(xi);
            nodeList.Add(xi, firstNode);
        }

        if (secondNode == null)
        {
            secondNode = new Node(yi);
            nodeList.Add(yi, secondNode);
        }

        firstNode.AddLink(secondNode);
        secondNode.AddLink(firstNode);
    }

    public static int CleanTree()
    {
        var count = 0;
        var idRoots = new List<int>();
        foreach (var node in nodeList.Values)
        {
            if (node.Links.Count == 1)
            {
                idRoots.Add(node.Id);
            }
        }

        foreach (var idRoot in idRoots)
        {
            bool hasLowestDepth = false;
            var root = nodeList[idRoot];

            var parent = root.Links[0];
            if (parent.Links.Count >= 3)
            {
                foreach (var brother in parent.Links)
                {
                    if (brother.Links.Count >= 1)
                    {
                        hasLowestDepth = true;
                    }
                }
            }
            if (hasLowestDepth)
            {
                count++;
                root.RemoveLink();
                nodeList.Remove(idRoot);
            }
        }

        Console.Error.WriteLine("Number of nodes to remove:{0}", count);
        return count;
    }
}

public class Node
{
    public int Id { get; private set; }
    public List<Node> Links { get; private set; }

    public Node(int id)
    {
        Id = id;
        Links = new List<Node>();
    }
    
    public void AddLink(Node child)
    {
        if (!Links.Exists(n => n.Id == child.Id))
        {
            Links.Add(child);
        }
    }

    public void RemoveLink()
    {
        foreach(var node in Links)
        {
            node.Links.Remove(this);
        }
        Links.Clear();
    }

    public static int CountDepth(Node node, Node parent)
    {
        int max = 0;
        foreach(var child in node.Links)
        {
            if (child != parent)
            {
                var current = CountDepth(child, node) + 1;
                if (current > max)
                {
                    max = current;
                }
            }
        }

        return max;
    }

}
