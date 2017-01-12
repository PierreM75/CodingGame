using System;
using System.Collections.Generic;
using System.Linq;

class Solution
{
    static int _lowerDiff = int.MaxValue;
    static readonly SortedSet<int> Horses = new SortedSet<int>();
    static void Main(string[] args)
    {
        var count = Math.Min(int.Parse(Console.ReadLine()), 99999);
        for (var i = 0; i < count; i++)
        {
            var power = int.Parse(Console.ReadLine());
            if (Horses.Contains(power))
            {
                Console.WriteLine(0);
                return;
            }
            else if (power < 1 || power > 10000000)
            {
                continue;
            }

            Horses.Add(power);
        }

        GetLowerDifference();

        Console.WriteLine(_lowerDiff);
    }

    private static void GetLowerDifference()
    {
        if (Horses.Count == 0 || Horses.Count == 1)
        {
            _lowerDiff = 0;
            return;
        }
        
        var previousHorse = Horses.First();
        Horses.Remove(previousHorse);
        foreach (var horse in Horses)
        {
            ComparePower(horse, previousHorse);
            previousHorse = horse;
        }
    }

    private static void ComparePower(int horse, int lastHorse)
    {
        if (horse == lastHorse)
        {
            return;
        }
        var currentDiff = Math.Abs(horse - lastHorse);
        if (_lowerDiff > currentDiff)
        {
            _lowerDiff = currentDiff;
        }
    }
}
