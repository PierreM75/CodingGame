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
        int n = int.Parse(Console.ReadLine()); // the number of temperatures to analyse
        string temps = Console.ReadLine(); // the n temperatures expressed as integers ranging from -273 to 5526
        
        var list = string.IsNullOrEmpty(temps)
        ? new List<int>()
        : temps.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).Select(o => int.Parse(o)).ToList();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.Error.WriteLine(string.Join(" ", list));
        var temperature = list.FirstOrDefault();
        foreach (int temp in list)
        {
            if (Math.Abs(temp) < Math.Abs(temperature))
            {
                temperature = temp;
            }
            if (Math.Abs(temp) == Math.Abs(temperature))
            {
                if (temp > 0)
                {
                    temperature = temp;
                }
            }
        }

        Console.WriteLine(temperature);
    }
}
