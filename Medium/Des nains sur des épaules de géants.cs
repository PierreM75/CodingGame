using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static Dictionary<int, Node> nodes = new Dictionary<int, Node>();

    static void Main(string[] args)
    {
        //var list = new List<string> { "1 2", "1 3", "3 4" };
        int n = int.Parse(Console.ReadLine()); // the number of relationships of influence
        for (int i = 0; i < n; i++)
        {
            var inputs = Console.ReadLine().Split(' ');
            //var inputs = list[i].Split(' ');
            var x = int.Parse(inputs[0]); // a relationship of influence between two people (x influences y)
            var y = int.Parse(inputs[1]);

            var nodeX = GetNode(x);
            var nodeY = GetNode(y);
            Console.Error.WriteLine("{0} {1}", x, y);

            nodeX.Children.Add(nodeY);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        var count = GetMaxCount();

        // The number of people involved in the longest succession of influences
        Console.WriteLine(count);
    }

    private static int GetMaxCount()
    {
        var maxCount = 0;
        foreach (var node in nodes.Values)
        {
            var currentCount = Count(node) + 1;
            if (maxCount < currentCount)
            {
                maxCount = currentCount;
            }
        }

        return maxCount;
    }

    private static int Count(Node node)
    {
        if (node.Children.Count == 0)
        {
            return 0;
        }

        var maxCount = 0;
        foreach (Node child in node.Children)
        {
            var currentCount = Count(child) + 1;
            if (maxCount < currentCount)
            {
                maxCount = currentCount;
            }
        }

        return maxCount;
    }

    private static Node GetNode(int id)
    {
        Node node;
        if (!nodes.TryGetValue(id, out node))
        {
            node = new Node(id);
            nodes.Add(id, node);
        }

        return node;
    }
}

public class Node
{
    public int Id { get; private set; }

    public List<Node> Children { get; set; }

    public Node(int id)
    {
        this.Id = id;
        this.Children = new List<Node>();
    }
}
