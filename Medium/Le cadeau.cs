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
    static void Main(string[] args)
    {
        var N = int.Parse(Console.ReadLine());
        Console.Error.WriteLine("number " + N);
        
        var C = int.Parse(Console.ReadLine());
        Console.Error.WriteLine("cost " + C);
        
        var list = new List<int>();
        var result = new List<int>();

        for (var i = 0; i < N; i++)
        {
            var B = int.Parse(Console.ReadLine());
            list.Add(B);
            Console.Error.WriteLine("participation " + B);
        }

        list.Sort();
        var sum = list.Sum();
        if (sum < C)
        {
            Console.WriteLine("IMPOSSIBLE");
        }
        else
        {
            var total = C;
            
            for (var i = 0; i < N; i++)
            {
                var remain = list.Sum();
                var donneur = list[0];
                var mise = Math.Min(Math.Min(total, remain) / list.Count, donneur);
                result.Add(mise);
                list.Remove(donneur);
                total -= mise;
            }
        }

        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
    }
}
