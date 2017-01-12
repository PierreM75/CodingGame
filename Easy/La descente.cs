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
class Player
{
    static void Main(string[] args)
    {

        // game loop
        while (true)
        {
            int selectedMountainId = 0;
            int selectedMountainH = 0;   
            
            for (int i = 0; i < 8; i++)
            {
                int mountainH = int.Parse(Console.ReadLine()); // represents the height of one mountain, from 9 to 0.
                if(mountainH >= selectedMountainH)
                {
                    selectedMountainId = i;
                    selectedMountainH = mountainH;
                }   
            }
            
            // Write an action using Console.WriteLine()
            // To debug: Console.Error.WriteLine("Debug messages...");

            Console.WriteLine(selectedMountainId); // The number of the mountain to fire on.
        }
    }
}
