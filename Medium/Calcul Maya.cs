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
        string[] inputs = Console.ReadLine().Split(' ');
        int L = int.Parse(inputs[0]);
        int H = int.Parse(inputs[1]);


        var mayas = new List<MayaFigure>();
        for (var number = 0; number < 20; number++)
        {
            var maya = new MayaFigure
            {
                Number = number,
                Figure = new List<string>()
            };
            mayas.Add(maya);
        }

        for (var j = 0; j < H; j++)
        {
            var numeral = Console.ReadLine();
            for (var number = 0; number < 20; number++)
            {
                var maya = mayas.First(x => x.Number == number);
                maya.Figure.Add(numeral.Substring(number * L, L));
            }
        }

        var s1Result = GetMayaFigures(H, mayas);
        var s2Result = GetMayaFigures(H, mayas);
        var operation = Console.ReadLine();

        // Write an action using Console.WriteLine()
        // To debug: Console.Error.WriteLine("Debug messages...");
        var s1Value = GetSum(s1Result);
        var s2Value = GetSum(s2Result);
        var result = GetOperation(operation, s1Value, s2Value);

        var mayaResult = DecimalToMaya(result, mayas);
        
        foreach (var maya in mayaResult)
        {
            foreach (var figure in maya.Figure)
            {
                Console.WriteLine(figure);
            }
        }
    }

    private static Dictionary<int, MayaFigure> GetMayaFigures(int largeur, List<MayaFigure> mayas)
    {
        var mayaFigures = new Dictionary<int, MayaFigure>();
        var longueur = int.Parse(Console.ReadLine());
        var power = longueur / largeur -1;
        for (var i = 0; i < longueur / largeur; i++)
        {
            var figure = new List<string>();
            for (var j = 0; j < largeur; j++)
            {
                var numeral = Console.ReadLine();
                figure.Add(numeral);
            }

            mayaFigures.Add(power, Match(figure, mayas));
            power--;
        }
        return mayaFigures;
    }

    private static long GetOperation(string operation, long s1Value, long s2Value)
    {
        long result = 0;
        switch (operation)
        {
            case "+":
                result = s1Value + s2Value;
                break;
            case "-":
                result = s1Value - s2Value;
                break;
            case "/":
                result = s1Value / s2Value;
                break;
            case "*":
                result = s1Value * s2Value;
                break;
        }
        
        Console.Error.WriteLine("Maya Calcul: {0} {1} {2} = {3}", s1Value, operation, s2Value, result);

        return result;
    }

    private static long GetSum(Dictionary<int, MayaFigure> s1Result)
    {
        long total = 0;
        foreach (var mayaFigure in s1Result)
        {
            var value = (long)mayaFigure.Value.Number * (long)Math.Pow(20, mayaFigure.Key);
            Console.Error.WriteLine("{0} * 20 Power {1} = {2}", mayaFigure.Value.Number, mayaFigure.Key, value);
            total += value;
        }
        return total;
    }

    private static List<MayaFigure> DecimalToMaya(long result, List<MayaFigure> mayas)
    {
        var mayaFigure = new List<MayaFigure>();

        int power = 0;
        long value = long.MaxValue;

        while (value > 20 || power > 0)
        {
            value = result / (long)Math.Pow(20, power);
            if (value > 20)
            {
                power++;
            }
            else
            {
                mayaFigure.Add(mayas.First(x => x.Number == (int)value));
                Console.Error.WriteLine("{0} * 20^{1}", value, power);
                result = result - (long)value * (long)Math.Pow(20, power);
                if(power > 0)
                {
                    power--;            
                    value = long.MaxValue;    
                }
            }
        }

        return mayaFigure;
    }

    private static MayaFigure Match(List<string> figure, List<MayaFigure> mayas)
    {
        MayaFigure maya = null;
        foreach (var mayaFigure in mayas)
        {
            if (Compare(mayaFigure.Figure,figure))
            {
                maya = mayaFigure;
            }
        }

        return maya;
    }

    private static bool Compare(List<string> figure1, List<string> figure2)
    {
        var result = true;
        for (var i = 0; i < figure1.Count; i++)
        {
            if (figure1[i] != figure2[i])
            {
                result = false;
            }
        }

        return result;        
    }

    public class MayaFigure
    {
        public int Number { get; set; }

        public List<string> Figure { get; set; }
    }
}
