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
        string MESSAGE = Console.ReadLine();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");

        Console.WriteLine(ExtractWord(MESSAGE));
    }
    
    private static string ExtractWord(string text)
    {
        var ascii = Encoding.ASCII.GetBytes(text);
        var binary = string.Empty;
        foreach (var b in ascii)
        {
            binary += int.Parse(Convert.ToString(b, 2)).ToString("D7");
        }
        var chuckNorris = ConvertBinaryToChuck(binary);
        Console.Error.WriteLine("{0} -> {1} -> {2}", text, binary,chuckNorris);
        
        return chuckNorris;
    }

    private static string ConvertBinaryToChuck(string binary)
    {
        var chuck = string.Empty;
        char currentValue = '0';
        for (var i = 0; i < binary.Length; i++)
        {
            if (i == 0 || binary[i] != currentValue)
            {
                currentValue = binary[i];
                chuck += ChuckType(currentValue, i);
            }
            chuck += "0";
        }
        return chuck;
    }

    private static string ChuckType(char value, int index)
    {
        return string.Format("{0}{1}", index == 0 ? "" : " ", value == '0' ? "00 " : "0 ");
    }
}
