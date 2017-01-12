using System;
using System.Linq;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Solution
{
    static void Main(string[] args)
    {
        int R = int.Parse(Console.ReadLine());
        int L = int.Parse(Console.ReadLine());

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        var result = R.ToString();

        for (var i = 1; i < L; i++)
        {
            var currentNumber = int.MinValue;
            var currentCount = 0;
            var first = true;
            var nextResult = string.Empty;

            var numbers = result.Split(' ').Select(int.Parse).ToList();
            foreach (var number in numbers)
            {
                if (first)
                {
                    currentNumber = number;
                    currentCount++;
                    first = false;
                    continue;
                }

                if (number == currentNumber)
                {
                    currentCount++;
                }
                else
                {
                    nextResult += string.Format("{0} {1} ", currentCount, currentNumber);
                    currentNumber = number;
                    currentCount = 1;
                }
            }

            nextResult += string.Format("{0} {1} ", currentCount, currentNumber);
            result = nextResult.Trim();
        }

        Console.Error.WriteLine(result.Trim());
        Console.WriteLine(result.Trim());
    }
}
