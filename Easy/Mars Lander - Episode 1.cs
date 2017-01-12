using System;
using System.Collections.Generic;

/**
* Auto-generated code below aims at helping you parse
* the standard input according to the problem statement.
**/
class Player
{
    private static int landZone;
    
    static void Main(string[] args)
    {
        landZone = -1;
        string[] inputs;
        int surfaceN = int.Parse(Console.ReadLine()); // the number of points used to draw the surface of Mars.
        for (int i = 0; i < surfaceN; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            var landX = int.Parse(inputs[0]); // X coordinate of a surface point. (0 to 6999)
            var landY = int.Parse(inputs[1]);
            if(landZone == -1 || landZone == landY)
            {
                landZone = landY;
            }
        }
        
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

            if(Y > landZone + 1200 && vSpeed > -48)
            {
                power =1;
            }
            else if (Math.Abs(vSpeed) <= 40 - 2)
            {
                power =2;
            }
            else 
            {
                power = 4;
            }
            
            Console.WriteLine("0 {0}", power);
        }
    }
}
