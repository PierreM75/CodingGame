using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static readonly List<Coord> Houses = new List<Coord>();

    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            var coord = new Coord { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1]) };
            Houses.Add(coord);            
        }

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        var xMin = Houses.First().X;
        var xMax = Houses.First().X;    
        var sortedList = Houses.OrderBy(coord => coord.Y).ToList();
        foreach (var house in sortedList)
        {
            Console.Error.WriteLine(house);

            if (xMin > house.X)
            {
                xMin = house.X;
            }
            if (xMax < house.X)
            {
                xMax = house.X;
            }
        }

        long yLength = 0;
        var middle = (int)Math.Ceiling((double)Houses.Count / 2);            
        var yAverage = sortedList[middle-1].Y;
        
        Console.Error.WriteLine("xMin:{0}, xMax:{1}, count:{2}, middle:{3}, yAverage:{4}", xMin, xMax, Houses.Count, middle, yAverage);
        var xLength = xMax - xMin;        
        foreach (var house in Houses)
        {
            yLength += Math.Abs(yAverage - house.Y);
        }
        
        Console.WriteLine(xLength + yLength);
    }
}

class Coord
{
    public int X { get; set; }
    public int Y { get; set; }
    public override string ToString()
    {
        return string.Format("{0} {1}", this.X, this.Y);
    }
}
