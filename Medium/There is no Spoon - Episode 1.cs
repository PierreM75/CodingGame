using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Don't let the machines win. You are humanity's last hope...
 **/
class Player
{
    private static bool[] matrix;
    private static int width;
    private static int height;

    static void Main(string[] args)
    {
        width = int.Parse(Console.ReadLine()); // the number of cells on the X axis
        height = int.Parse(Console.ReadLine()); // the number of cells on the Y axis

        matrix = new bool[width * height];
        Console.Error.WriteLine("i:{0}, j:{1}", width, height);

        for (int j = 0; j < height; j++)
        {
            var line = Console.ReadLine(); //lines[j];
            SetMatrix(j, line);
            // Console.Error.WriteLine(line);
        }

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                if (matrix[width * j + i])
                {
                    var coord = new Coord(i, j);
                    var coordRight = FindFirstRightNode(coord);
                    var coordBottom = FindFirstBottomNode(coord);
                    Console.WriteLine("{0} {1} {2}", coord, coordRight, coordBottom);
                }
            }
        }

        Console.ReadLine();
    }

    private static void SetMatrix(int j, string line)
    {
        for (int i = 0; i < width; i++)
        {
            matrix[width * j + i] = line[i] == '0';
        }
    }

    private static Coord FindFirstBottomNode(Coord coord)
    {
        if (coord.J + 1 == height)
        {
            return new Coord(-1, -1);
        }

        for (int j = coord.J + 1; j < height; j++)
        {
            if (matrix[width * j + coord.I])
            {
                return new Coord(coord.I, j);
            }
        }

        return new Coord(-1, -1);
    }

    private static Coord FindFirstRightNode(Coord coord)
    {
        if (coord.I + 1 == width)
        {
            return new Coord(-1, -1);
        }

        for (int i = coord.I + 1; i < width; i++)
        {
            if (matrix[width * coord.J + i])
            {
                return new Coord(i, coord.J);
            }
        }

        return new Coord(-1, -1);
    }

    public class Coord
    {
        private int i;
        private int j;

        public Coord(int i, int j)
        {
            this.i = i;
            this.j = j;
        }

        public int I
        {
            get { return i; }
        }

        public int J
        {
            get { return j; }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", i, j);
        }
    }
}
