using System;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private static Coord lastCoord;
    private static int down;
    private static int up;
    private static int left;
    private static int right;

    static void Main(string[] args)
    {
        ConfigureGame();
        //ConfigureGame(4, 8, 40, 2, 3);
        //ConfigureGame(25, 33, 49, 2, 29);
        //ConfigureGame(40, 60, 32, 6, 6);
        //ConfigureGame(1, 80, 6, 0, 1);
        //ConfigureGame(100, 100, 7, 5, 98);
        //ConfigureGame(9999, 9999, 14, 54, 77);

        // game loop
        while (true)
        {
            var bombDir = Console.ReadLine(); // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
            var direction = TransformToVector(bombDir);
            lastCoord = ApplyDeformation(direction);
            Console.Error.WriteLine("Direction:{0}, new Coord: {1}", direction.ToString(), lastCoord.ToString());
            // the location of the next window Batman should jump to.
            Console.WriteLine(lastCoord.ToString());
        }
    }

    private static void ConfigureGame(int i, int j, int n, int x0, int y0)
    {
        Console.Error.WriteLine("Game i:{0}, j:{1}, n:{2}, x0:{3}, y0:{4} ", i, j, n, x0, y0);
        lastCoord = new Coord(x0, y0);

        up = 0;
        down = j - 1;
        left = 0;
        right = i - 1;
    }

    private static void ConfigureGame()
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        var W = int.Parse(inputs[0]); // width of the building.
        var H = int.Parse(inputs[1]); // height of the building.
        var N = int.Parse(Console.ReadLine()); // maximum number of turns before game over.
        inputs = Console.ReadLine().Split(' ');
        var X0 = int.Parse(inputs[0]);
        var Y0 = int.Parse(inputs[1]);
        ConfigureGame(W, H, N, X0, Y0);
    }

    private static Coord TransformToVector(string bombDir)
    {
        // the direction of the bombs from batman's current location (U, UR, R, DR, D, DL, L or UL)
        switch (bombDir)
        {
            case "U":
                return new Coord(0, -1);
            case "UR":
                return new Coord(1, -1);
            case "R":
                return new Coord(1, 0);
            case "DR":
                return new Coord(1, 1);
            case "D":
                return new Coord(0, 1);
            case "DL":
                return new Coord(-1, 1);
            case "L":
                return new Coord(-1, 0);
            case "UL":
                return new Coord(-1, -1);
        }
        return new Coord(0, 0);
    }

    private static Coord ApplyDeformation(Coord direction)
    {
        CleanXAxis(direction);
        CleanYAxis(direction);

        var floatx = (left + right) / 2.0;
        var floaty = (up + down) / 2.0;

        Console.Error.WriteLine("Up:{0},Down:{1},Left:{2},Right:{3}", up, down, left, right);
        var floorx = direction.x > 0 ? Math.Ceiling(floatx) : Math.Floor(floatx);
        var floory = direction.y > 0 ? Math.Ceiling(floaty) : Math.Floor(floaty);

        return new Coord(
            direction.x == 0 ? lastCoord.x : (int)floorx,
            direction.y == 0 ? lastCoord.y : (int)floory);
    }

    private static void CleanYAxis(Coord direction)
    {
        if (direction.y == 0)
        {
            up = down = lastCoord.y;
        }
        if (direction.y > 0)
        {
            up = lastCoord.y;
        }
        if (direction.y < 0)
        {
            down = lastCoord.y;
        }
    }

    private static void CleanXAxis(Coord direction)
    {
        if (direction.x == 0)
        {
            left = right = lastCoord.x;
        }
        if (direction.x > 0)
        {
            left = lastCoord.x;
        }
        if (direction.x < 0)
        {
            right = lastCoord.x;
        }
    }
}

public class Coord
{
    public int x { get; private set; }

    public int y { get; private set; }

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("{0} {1}", this.x, this.y);
    }
}
