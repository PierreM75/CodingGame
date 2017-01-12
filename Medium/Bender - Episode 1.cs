using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
public class Solution
{
    static void Main(string[] args)
    {
        var map = GetParameters();
        var bender = new Bender { Coord = map.FindObstacle(MapObstacle.Start), Direction = Direction.South };
        List<string> routes = new List<string>();

        while (!bender.IsSuicided)
        {
            var currentObstacle = map.GetObstacle(bender.Coord);
            if (currentObstacle == MapObstacle.SuicidCabine)
            {
                Console.Error.WriteLine(bender.Coord  + " " + "Obstacle SuicidCabine");
                bender.IsSuicided = true;
            }
            if (currentObstacle == MapObstacle.Hooligan)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle Hooligan");
                bender.IsHooligan = !bender.IsHooligan;
            }
            if (currentObstacle == MapObstacle.Inverter)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle Inverter");
                bender.IsReverted = !bender.IsReverted;
            }
            if (currentObstacle == MapObstacle.Teleporter)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle Teleporter");
                bender.Coord = map.FindObstacle(MapObstacle.Teleporter, bender.Coord);
            }
            if (currentObstacle == MapObstacle.TrajectoryToEast)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle TrajectoryToEast");
                bender.Direction = Direction.East;
            }
            if (currentObstacle == MapObstacle.TrajectoryToNorth)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle TrajectoryToNorth");
                bender.Direction = Direction.North;
            }
            if (currentObstacle == MapObstacle.TrajectoryToWest)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle TrajectoryToWest");
                bender.Direction = Direction.West;
            }
            if (currentObstacle == MapObstacle.TrajectoryToSouth)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle TrajectoryToSouth");
                bender.Direction = Direction.South;
            }
            if (currentObstacle == MapObstacle.None)
            {
                // Console.Error.WriteLine(bender.Coord + " " + "Obstacle None");
            }
            if (currentObstacle == MapObstacle.Start)
            {
                Console.Error.WriteLine(bender.Coord + " " + "Obstacle Start");
            }

            var nextCoord = map.NextCoord(bender.Coord, bender.Direction);
            var nextObstacle = map.GetObstacle(nextCoord);
            if (nextObstacle == MapObstacle.MurCassable && bender.IsHooligan)
            {
                Console.Error.WriteLine("mur is distroyed, keep direction");
                map.SetObstacle(nextCoord, MapObstacle.None);
                bender.Routes.Clear();
            }

            bender.Direction = IsMur(nextObstacle, bender) ? !bender.IsReverted ? Direction.South : Direction.West : bender.Direction;
            var success = false;
            
            while (!success)
            {
                nextCoord = map.NextCoord(bender.Coord, bender.Direction);
                nextObstacle = map.GetObstacle(nextCoord);

                if (IsMur(nextObstacle, bender))
                {
                    Console.Error.WriteLine(nextCoord + " " + "Obstacle is mur, must change direction");
                    bender.Direction = NextDirection(bender);
                }
                else
                {
                    success = true;
                }
            }

            if (!bender.IsSuicided)
            {
                bender.Coord = nextCoord;
                routes.Add(bender.Direction.ToString().ToUpper());
                
                if (!bender.AddRoute())
                {
                    routes.Clear();
                    routes.Add("LOOP");
                    bender.IsSuicided = true;
                }
            }
        }

        foreach (var route in routes)
        {
            Console.WriteLine(route);
        }
    }

    private static bool IsMur(MapObstacle nextObstacle, Bender bender)
    {
        if (nextObstacle == MapObstacle.MurNonCassable)
        {
            return true;
        }

        if (nextObstacle == MapObstacle.MurCassable && !bender.IsHooligan)
        {
            return true;
        }
        
        return false;
    }

    private static Direction NextDirection(Bender bender)
    {
        Direction nextDirection = Direction.None;
        switch (bender.Direction)
        {
            case Direction.South:
            {
                nextDirection = !bender.IsReverted ? Direction.East : Direction.West;
                break;
            }
            case Direction.East:
            {
                nextDirection = !bender.IsReverted ? Direction.North : Direction.South;
                break;
            }
            case Direction.North:
            {
                nextDirection = !bender.IsReverted ? Direction.West : Direction.East;
                break;
            }
            case Direction.West:
            {
                nextDirection = !bender.IsReverted ? Direction.South : Direction.North;
                break;
            }
        }

        return nextDirection;
    }

    private static Map GetParameters()
    {
        //var inputLine = "10 10";
        //var arrayLine = new string[]
        //                {
        //                    "##########", "#        #", "#  S   W #", "#        #", "#  $     #", "#        #",
        //                    "#@       #", "#        #", "#E     N #", "##########"
        //                };

        //var inputs = inputLine.Split(' ');
        var inputs = Console.ReadLine().Split(' ');
        var L = int.Parse(inputs[0]);
        var C = int.Parse(inputs[1]);
        var map = new Map(C, L);
        for (var j = 0; j < L; j++)
        {
            // var row = arrayLine[j];
            var row = Console.ReadLine();
            for (var i = 0; i < C; i++)
            {
                var coord = new Coord(i, j);
                var constraint = Map.CharToObstacle(row[i]);
                map.SetObstacle(coord, constraint);
            }
            Console.Error.WriteLine(row);
        }
        return map;
    }
}

public enum Direction
{
    South,

    East,

    North,

    West,

    None
}

public enum MapObstacle
{
    Start,

    MurNonCassable,

    MurCassable,

    TrajectoryToSouth,

    TrajectoryToEast,

    TrajectoryToNorth,

    TrajectoryToWest,

    Inverter,

    Hooligan,

    Teleporter,

    SuicidCabine,

    None
}

public class Map
{
    private readonly MapObstacle[] coords;

    private readonly int sizeX;

    private readonly int sizeY;

    public Map(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.coords = new MapObstacle[this.sizeX * this.sizeY];
    }

    public static MapObstacle CharToObstacle(char obstacle)
    {
        switch (obstacle)
        {
            case '@':
                return MapObstacle.Start;
            case '$':
                return MapObstacle.SuicidCabine;
            case '#':
                return MapObstacle.MurNonCassable;
            case 'X':
                return MapObstacle.MurCassable;
            case 'S':
                return MapObstacle.TrajectoryToSouth;
            case 'E':
                return MapObstacle.TrajectoryToEast;
            case 'N':
                return MapObstacle.TrajectoryToNorth;
            case 'W':
                return MapObstacle.TrajectoryToWest;
            case 'I':
                return MapObstacle.Inverter;
            case 'B':
                return MapObstacle.Hooligan;
            case 'T':
                return MapObstacle.Teleporter;
        }

        return MapObstacle.None;
    }

    public void SetObstacle(Coord coord, MapObstacle constraint)
    {
        this.coords[coord.Y * this.sizeX + coord.X] = constraint;
    }

    public MapObstacle GetObstacle(Coord coord)
    {
        return this.coords[coord.Y * this.sizeX + coord.X];
    }

    public Coord FindObstacle(MapObstacle obstacle, Coord excludeCoord = null)
    {
        for (var j = 0; j < this.sizeY; j++)
        {
            for (var i = 0; i < this.sizeX; i++)
            {
                var isCoordExcluded = excludeCoord != null && excludeCoord.X == i && excludeCoord.Y == j;
                if (this.coords[j * this.sizeX + i] == obstacle && !isCoordExcluded)
                {
                    return new Coord(i, j);
                }
            }
        }

        return null;
    }

    public Coord NextCoord(Coord currentCoord, Direction direction)
    {
        Coord coord = null;
        switch (direction)
        {
            case Direction.South:
                {
                    coord = new Coord(currentCoord.X, currentCoord.Y + 1);
                    break;
                }
            case Direction.East:
                {
                    coord = new Coord(currentCoord.X + 1, currentCoord.Y);
                    break;
                }
            case Direction.North:
                {
                    coord = new Coord(currentCoord.X, currentCoord.Y - 1);
                    break;
                }
            case Direction.West:
                {
                    coord = new Coord(currentCoord.X - 1, currentCoord.Y);
                    break;
                }
        }

        if (coord == null || coord.X < 0 || coord.X >= this.sizeX || coord.Y < 0 || coord.Y >= this.sizeY)
        {
            return null;
        }

        return coord;
    }
}

public class Bender
{
    public Bender()
    {
        this.IsHooligan = false;
        this.IsReverted = false;
        this.IsSuicided = false;
        this.Direction = Direction.None;
        this.Routes = new List<string>();
    }

    public Coord Coord { get; set; }

    public Direction Direction { get; set; }

    public bool IsSuicided { get; set; }

    public bool IsHooligan { get; set; }

    public bool IsReverted { get; set; }

    public List<string> Routes { get; set; }

    public bool AddRoute()
    {
        var route = string.Format("{0}{1}{2}{3}", this.Coord, this.Direction, this.IsHooligan, this.IsReverted);
        if (this.Routes.Contains(route))
        {
            return false;
        }

        this.Routes.Add(route);
        return true;
    }
}

public class Coord
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public Coord(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public override string ToString()
    {
        return string.Format("x: {0}, y: {1}", this.X, this.Y);
    }
}
