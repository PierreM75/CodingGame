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
    private static List<Box> _boxes = new List<Box>();
    static void Main(string[] args)
    {
        var lon = DegreeToRadian(Console.ReadLine());
        var lat = DegreeToRadian(Console.ReadLine());
        var number = int.Parse(Console.ReadLine());
        for (var i = 0; i < number; i++)
        {
            var defib = Console.ReadLine();
            var datas = defib.Split(new[] {";"}, StringSplitOptions.None);
            _boxes.Add(new Box(datas[1], DegreeToRadian(datas[4]), DegreeToRadian(datas[5])));
        }

        double nearestDistance = double.MaxValue;
        string nearestBox = string.Empty;
        foreach (var box in _boxes)
        {
            var distance = box.Distance(lat, lon);
            if (nearestDistance > distance)
            {
                nearestBox = box.Name;
                nearestDistance = distance;
            }
        }

        Console.WriteLine(nearestBox);
        Console.ReadLine();
    }

    private static double DegreeToRadian(string value)
    {
        var degree = double.Parse(value.Replace(",", "."));
        return (degree*Math.PI)/180;
    }
}

internal class Box
{
    public Box(string name, double longitude, double latitude)
    {
        Name = name;
        Longitude = longitude;
        Latitude = latitude;
    }

    public string Name { get; private set; }
    public double Longitude { get; private set; }
    public double Latitude { get; private set; }

    public double Distance(double latitude, double longitude)
    {
        var x = (longitude - Longitude)*Math.Cos((latitude + Latitude)/2);
        var y = latitude - Latitude;

        return Math.Sqrt(Math.Pow(x,2) + Math.Pow(y, 2)) * 6371;
    }
}
