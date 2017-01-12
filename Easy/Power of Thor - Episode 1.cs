using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 * ---
 * Hint: You can use the debug stream to print initialTX and initialTY, if Thor seems not follow your orders.
 **/
class Player
{
    static void Main(string[] args)
    {
        string[] inputs = Console.ReadLine().Split(' ');
        int lightX = int.Parse(inputs[0]); // the X position of the light of power
        int lightY = int.Parse(inputs[1]); // the Y position of the light of power
        int initialTX = int.Parse(inputs[2]); // Thor's starting X position
        int initialTY = int.Parse(inputs[3]); // Thor's starting Y position

        int currentPositionX = initialTX;
        int currentPositionY = initialTY;

        // game loop
        while (true)
        {
            int remainingTurns = int.Parse(Console.ReadLine()); // The remaining amount of turns Thor can move. Do not remove this line.
            
            bool mooveRight = currentPositionX < lightX;
            bool mooveLeft = currentPositionX > lightX;
            bool mooveDown = currentPositionY < lightY;
            bool mooveUp = currentPositionY > lightY;
            
            string way;
            if(mooveUp && !mooveLeft && !mooveDown && !mooveRight)
            {
                 way = "N";
                 currentPositionY--;
            }
            else if(mooveUp && !mooveLeft && !mooveDown && mooveRight)
            {
                 way = "NE";
                 currentPositionY--;
                 currentPositionX++;
            }
            else if(!mooveUp && !mooveLeft && !mooveDown && mooveRight)
            {
                 way = "E";
                 currentPositionX++;
            }
            else if(!mooveUp && !mooveLeft && mooveDown && mooveRight)
            {
                 way = "SE";
                 currentPositionY++;
                 currentPositionX++;
            }
            else if(!mooveUp && !mooveLeft && mooveDown && !mooveRight)
            {
                 way = "S";
                 currentPositionY++;                 
            }
            else if(!mooveUp && mooveLeft && mooveDown && !mooveRight)
            {
                 way = "SW";
                 currentPositionY++;
                 currentPositionX--;
            }
            else if(!mooveUp && mooveLeft && !mooveDown && !mooveRight)
            {
                 way = "W";
                 currentPositionX--;
            }
            else if(!mooveUp && !mooveLeft && mooveDown && mooveRight)
            {
                 way = "NW";
                 currentPositionY--;
                 currentPositionX--;
            }
            else
            {
                 way = "N";
            }
            // A single line providing the move to be made: N NE E SE S SW W or NW
            Console.WriteLine(way);
        }
    }
}
