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
    private static Room[] map;
    private static int sizeX;
    private static int sizeY;

    static void Main(string[] args)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        sizeX = int.Parse(inputs[0]); // number of columns.
        sizeY = int.Parse(inputs[1]); // number of rows.
        map = new Room[sizeX * sizeY];
        for (int j = 0; j < sizeY; j++)
        {
            var rooms = Console.ReadLine().Split(' '); // represents a line in the grid and contains W integers. Each integer represents one room of a given type.
            for (int i = 0; i < sizeX; i++)
            {
                Console.Error.WriteLine("i:{0}, j:{1}, type:{2}", i, j, rooms[i]);
                map[sizeX * j + i] = Room.CreateRoom(int.Parse(rooms[i]));
            }
        }
        int EX = int.Parse(Console.ReadLine()); // the coordinate along the X axis of the exit (not useful for this first mission, but must be read).

        // game loop
        while (true)
        {
            inputs = Console.ReadLine().Split(' ');
            int XI = int.Parse(inputs[0]);
            int YI = int.Parse(inputs[1]);
            string POS = inputs[2];

            // Write an action using Console.WriteLine()
            Console.Error.WriteLine("XI:{0}, YI:{1}, POS:{2}", XI, YI, POS);
            var room = map[sizeX * YI + XI];
            var directionIn = (Direction)Enum.Parse(typeof(Direction), POS);
            var way = room.Ways.Find(w => w.In == directionIn);
            var directionOut = way.Out;

            switch (directionOut)
            {
                case Direction.LEFT:
                    XI--;
                    break;
                case Direction.BOTTOM:
                    YI++;
                    break;
                case Direction.RIGHT:
                    XI++;
                    break;
            }

            // One line containing the X Y coordinates of the room in which you believe Indy will be on the next turn.
            Console.Error.WriteLine(XI + " " + YI);
            Console.WriteLine(XI + " " + YI);
        }
    }
}

public class Room
{
    public int RoomType { get; private set; }
    public int RoomClass { get; private set; }
    public List<Way> Ways { get; private set; }
    public static Room CreateRoom(int roomType)
    {
        var ways = new List<Way>();
        var roomClass = 0;
        switch (roomType)
        {
            case 0:
                roomClass = 0;
                break;
            case 1:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                roomClass = 1;
                break;
            case 2:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.LEFT });
                roomClass = 2;
                break;
            case 3:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                roomClass = 2;
                break;
            case 4:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.LEFT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                roomClass = 3;
                break;
            case 5:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                roomClass = 3;
                break;
            case 6:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.LEFT });
                roomClass = 4;
                break;
            case 7:
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                roomClass = 4;
                break;
            case 8:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                roomClass = 4;
                break;
            case 9:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                roomClass = 4;
                break;
            case 10:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.LEFT });
                roomClass = 5;
                break;
            case 11:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.RIGHT });
                roomClass = 5;
                break;
            case 12:
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                roomClass = 5;
                break;
            case 13:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                roomClass = 5;
                break;
        }
        return new Room
        {
            Ways = ways,
            RoomType = roomType,
            RoomClass = roomClass
        };
    }
}

public class Way
{
    public Direction In { get; set; }
    public Direction Out { get; set; }
}

public enum Direction
{
    TOP,
    RIGHT,
    BOTTOM,
    LEFT
}
