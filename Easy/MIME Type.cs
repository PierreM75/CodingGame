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
        var mapper = new Dictionary<string, string>();
        var fileNames = new List<string>();
        var elements = int.Parse(Console.ReadLine()); // Number of elements which make up the association table.
        var files = int.Parse(Console.ReadLine()); // Number Q of file names to be analyzed.
        for (var i = 0; i < elements; i++)
        {
            var inputs = Console.ReadLine().Split(' ');
            var ext = ClearExtension(inputs[0]);
            var mime = ClearMime(inputs[1]);
            Console.Error.WriteLine(ext + " => " + mime);
            mapper.Add(ext, mime);
        }
        for (int i = 0; i < files; i++)
        {
            var fileName = Console.ReadLine(); // One file name per line.
            Console.Error.WriteLine(fileName);
            fileNames.Add(fileName);
        }
        
        Console.Error.WriteLine();
            
        foreach (var filename in fileNames)
        {
            var extension = string.IsNullOrEmpty(filename) || !filename.Contains(".")
                ? string.Empty
                : filename.Split(new[] { '.' }, StringSplitOptions.None).Last();
            var clearExt = ClearExtension(extension);

            var result = "UNKNOWN";
            if (mapper.ContainsKey(clearExt))
            {
                result = mapper[clearExt];
            }

            // Console.Error.WriteLine(filename + " => " + result);
            
            Console.WriteLine(result);
        }
    }

    private static string ClearMime(string input)
    {
        /*var res = input.Replace(" ", string.Empty);
        if (res.Length >= 50)
        {
            return res.Substring(0, 50);
        }
        return res;*/
        return input;
    }

    private static string ClearExtension(string input)
    {
        var res = input.Replace(" ", string.Empty);
        if (res.Length >= 10)
        {
            return res.Substring(0, 10);
        }
        return res.ToUpper();
    }
}
