using System;
using System.Collections.Generic;







/**
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/
class Player
{
    private static Coord landZoneMin;
    private static Coord landZoneMax;
    private static double gravity = 3.711;
    
    private static int Y_MARGIN = 100; 
    private static int maxhSpeed = 20;
    private static int maxvSpeed = 40;
    private static int maxSpeedMargin = 5;
    
    private static List<Coord> landZone;

    static void Main(string[] args)
    {
        string[] inputs;
        Configure();

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');

            int X = int.Parse(inputs[0]);
            int Y = int.Parse(inputs[1]);
            double hSpeed = double.Parse(inputs[2]); // the horizontal speed (in m/s), can be negative.
            double vSpeed = double.Parse(inputs[3]); // the vertical speed (in m/s), can be negative.
            int fuel = int.Parse(inputs[4]); // the quantity of remaining fuel in liters.
            int rotate = int.Parse(inputs[5]); // the rotation angle in degrees (-90 to 90).
            int power = int.Parse(inputs[6]); // the thrust power (0 to 4).

            var position = new Coord { X = X, Y = Y };
            
            if (!OverLandingZone(position)) 
            {
                if (goesInWrongDirection(position, hSpeed) || goesTooFastHorizontally(hSpeed))
                {
                    Console.Error.WriteLine("Going to Landing Zone : decrease Speed");
                    rotate = angleToSlow(hSpeed, vSpeed);
                    power = 4;
                }
                else if (goesTooSlowHorizontally(hSpeed))
                {
                    Console.Error.WriteLine("Going to Landing Zone : increase Speed");
                    rotate = angleToAimTarget(position);
                    power = 4;
                }
                else
                {
                    Console.Error.WriteLine("Going to Landing Zone");
                    rotate = 0;
                    power = powerToHover(vSpeed);
                }
            }
            else 
            {
                if (isFinishing(position))
                {
                    Console.Error.WriteLine("Landing Zone - Finish");
                    rotate = 0;
                    power = 4;
                }
                else if (hasSafeSpeed(hSpeed, vSpeed))
                {
                    Console.Error.WriteLine("Landing Zone - Safe");
                    rotate = 0;
                    power =2;
                }
                else 
                {
                    Console.Error.WriteLine("Landing Zone - Unsafe");
                    rotate = angleToSlow(hSpeed, vSpeed);
                    power = 4;
                }
            }
            
            Console.WriteLine("{0} {1}", rotate, power);
        }
    }

    private static void Configure()
    {
        landZone = new List<Coord>();
        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            var landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            var landY = int.Parse(inputs[1]);
            // Y coordinate of a surface point. By linking all the points together in a sequential fashion, you form the surface of Mars.
            landZone.Add(new Coord { X = landX, Y = landY });
        }

        landZoneMin = new Coord { X = -1, Y = -1 };
        landZoneMax = new Coord { X = -1, Y = -1 };
        foreach (var coord in landZone)
        {
            Console.Error.WriteLine("Checking Zone {0}", coord);
            if (coord.IsYEqual(landZoneMin))
            {
                landZoneMax = coord;
                break;
            }
            landZoneMin = coord;
        }
        
        Console.Error.WriteLine("Land Zone is {0}-{1}", landZoneMin.X, landZoneMax.X);
    }

    private static bool OutOfScope(Coord position)
    {
        var limits = position.Y < 0
               || position.Y > 3000
               || position.X < 0
               || position.X > 7000;

        for (var i = 0; i < landZone.Count - 1; i++)
        {
            if (position.X >= landZone[i].X && position.X < landZone[i + 1].X)
            {
                var coeff = (landZone[i + 1].Y - landZone[i].Y) / (landZone[i + 1].X - landZone[i].X);
                var penteY = (position.X - landZone[i].X) * coeff + landZone[i].Y;

                if (penteY >= position.Y)
                {
                    limits = true;
                }
            }
        }

        return limits;
    }
    
    private static bool OverLandingZone(Coord position)
    {
        return  landZoneMin.X <= position.X
             && position.X <= landZoneMax.X;
    }

    private static bool hasSafeSpeed(double hSpeed, double vSpeed)
    {
        return Math.Abs(hSpeed) <= maxhSpeed - maxSpeedMargin 
            && Math.Abs(vSpeed) <= maxvSpeed - maxSpeedMargin;
    }

    public static bool isFinishing(Coord position) 
    {
        return position.Y < landZoneMin.Y + Y_MARGIN;
    }
    
    public static bool goesInWrongDirection(Coord position, double hSpeed) 
    {
        return (position.X < landZoneMin.X && hSpeed < 0) 
            || (landZoneMax.X < position.X && hSpeed > 0);
    }
    
    public static bool goesTooFastHorizontally(double hSpeed) 
    {
        return Math.Abs(hSpeed) > 4 * maxhSpeed;
    }
    
    public static bool goesTooSlowHorizontally(double hSpeed) 
    {
        return Math.Abs(hSpeed) < 2 * maxhSpeed;
    }
    
    public static int angleToSlow(double hSpeed, double vSpeed) 
    {
        return (int) (Math.Atan((double)hSpeed / Math.Abs((double)vSpeed)) * (180.0 / Math.PI));
    }
    
    public static int angleToAimTarget(Coord position) 
    {
        var value = gravity / 4.0;
        var angle = Math.Acos(value) * (180.0 / Math.PI);
        if (position.X < landZoneMin.X)
        {
            return -(int)angle;
        }
        else if (landZoneMax.X < position.X)
        {
            return (int)angle;
        }
        else
        {
            return 0;
        }
    }
    
    public static int powerToHover(double vSpeed) 
    {
        return (vSpeed >= 0) ? 3 : 4;
    }

}

public class Coord
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool IsYEqual(Coord other)
    {
        return (this.Y == other.Y);
    }

    public override string ToString()
    {
        return string.Format("x:{0} y:{1}", this.X, this.Y);
    }
}
