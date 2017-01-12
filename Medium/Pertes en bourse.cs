using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    private static List<int> values = new List<int>();
    static void Main(string[] args)
    {
        int n = int.Parse(Console.ReadLine());
        string[] inputs = Console.ReadLine().Split(' ');
        for (int i = 0; i < n; i++)
        {
            int v = int.Parse(inputs[i]);
            values.Add(v);
        }

        int maxValue = 0;
        List<int> diffs = new List<int>();
        if (values.Count > 0)
        {
            maxValue = values[0];
            diffs.Add(0);
        }
        
        foreach(var value in values)
        {
            if(value < maxValue)
            {
                int diff = maxValue - value;
                diffs.Add(diff);
            }

            if(value > maxValue)
            {
                maxValue = value;
            }
        }

        var answer = 0;
        foreach(var value in diffs)
        {
            if(value > answer)
            {
                answer = value;
            }
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(-answer);
    }
}
