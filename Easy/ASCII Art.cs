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
    static Dictionary<char, int> _translate = new Dictionary<char, int>
        {
            {'A', 0},
            {'B', 1},
            {'C', 2},
            {'D', 3},
            {'E', 4},
            {'F', 5},
            {'G', 6},
            {'H', 7},
            {'I', 8},
            {'J', 9},
            {'K', 10},
            {'L', 11},
            {'M', 12},
            {'N', 13},
            {'O', 14},
            {'P', 15},
            {'Q', 16},
            {'R', 17},
            {'S', 18},
            {'T', 19},
            {'U', 20},
            {'V', 21},
            {'W', 22},
            {'X', 23},
            {'Y', 24},
            {'Z', 25},
            {'?', 26}
        };
        
    static void Main(string[] args)
    {
        int l = int.Parse(Console.ReadLine());
        int h = int.Parse(Console.ReadLine());
        string text = Console.ReadLine();

        for (var i = 0; i < h; i++)
        {
            var row = Console.ReadLine();
            var answer = ExtractWord(text.ToUpper(), row, l);
            Console.WriteLine(answer);
        }
    }
    
    private static string ExtractWord(string text, string row, int sizeX)
    {
        var answer = string.Empty;
        if (string.IsNullOrEmpty(text))
        {
            return answer;
        }

        foreach (var letter in text)
        {
            var position = !_translate.ContainsKey(letter) ? _translate['?'] : _translate[letter];
            answer += row.Substring(position * sizeX, sizeX);
        }

        return answer;
    }
}
