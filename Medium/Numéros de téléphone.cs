using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static readonly List<Digit> Digits = new List<Digit>();

    static void Main(string[] args)
    {
        int N = int.Parse(Console.ReadLine());
        for (int i = 0; i < N; i++)
        {
            var telephone = Console.ReadLine();
            Console.Error.WriteLine(telephone);
            InsertDigit(telephone);
        }

        var count = Count(Digits);

        // The number of elements (referencing a number) stored in the structure.
        Console.WriteLine(count);
    }

    private static void InsertDigit(string telephone)
    {
        var lastDigit = Digits;
        foreach (var number in telephone)
        {
            var digit = int.Parse(number.ToString());
            var currentDigit = lastDigit.Find(dgt => dgt.Number == digit);
            if (currentDigit == null)
            {
                currentDigit = new Digit(digit);
                lastDigit.Add(currentDigit);
            }

            lastDigit = currentDigit.NextDigits;
        }
    }

    private static int Count(List<Digit> digitLists)
    {
        if (digitLists.Count == 0)
        {
            return 0;
        }

        var sum = digitLists.Count;
        foreach (var number in digitLists)
        {
            sum += Count(number.NextDigits);
        }

        return sum;
    }
}

public class Digit
{
    public int Number { get; private set; }

    public List<Digit> NextDigits { get; private set; }

    public Digit(int digit)
    {
        this.Number = digit;
        this.NextDigits = new List<Digit>();
    }
}
