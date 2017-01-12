using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private static Coord exit;
    private static Room[] map;
    private static Dictionary<int, Room> rooms;
    private static int sizeX;
    private static int sizeY;

    static void Main(string[] args)
    {
        string[] inputs;
        GetParameters();

        // game loop
        while (true)
        {
            Coord indy;
            Direction indyDirection;
            List<Coord> rocks;
            List<Direction> rocksDirection;
            GetParamUser(out indy, out indyDirection, out rocks, out rocksDirection);

            List<string> indyActions = null;
            var indyRoom = GetRoomFromMap(indy);
            FindWay(indy, indyRoom.RoomType, indyRoom.CanRotate, indyDirection, out indyActions);
            indyActions.Add(string.Format("{0} {1} {2}", indy.X, indy.Y, indyRoom.RoomType));
            indyActions.Reverse();

            var rocksActions = new Dictionary<int, List<string>>();
            if (rocks.Count > 0)
            {
                int indexRock = 0;
                foreach (var rock in rocks)
                {
                    var rockRidrection = rocksDirection[indexRock];
                    var rockRoom = GetRoomFromMap(rock);
                    List<string> rockActions;
                    FindWay(rock, rockRoom.RoomType, rockRoom.CanRotate, rockRidrection, out rockActions);
                    rockActions.Add(string.Format("{0} {1} {2}", rock.X, rock.Y, rockRoom.RoomType));
                    rockActions.Reverse();
                    rocksActions[indexRock] = rockActions;
                    indexRock++;
                }
            }

            Coord coord;
            int roomType;
            IndyVsRock(indy, indyDirection, indyActions, rocks, rocksActions, out coord, out roomType);

            SendOutput(coord, roomType);
        }
    }

    private static void GetParameters()
    {
        string[] inputs;
        var sizeStr = Console.ReadLine();
        //var sizeStr = "10 8";
        inputs = sizeStr.Split(' ');
        sizeX = int.Parse(inputs[0]); // number of columns.
        sizeY = int.Parse(inputs[1]); // number of rows.
        map = new Room[sizeX * sizeY];

        //var roomsStr = new List<string>
        //{
        //    "0 0 -3 0 0",
        //    "0 0 2 0 0",
        //    "0 0 -3 0 0"
        //};

        var roomsStr = new List<string>
        {
            "0 -3 0 -3 0 -3 0 -3 -3 0",
            "0 7 -2 3 -2 2 -2 3 11 0",
            "0 -7 -2 -2 -2 -2 2 -2 2 -2",
            "0 6 -2 -2 -2 -2 -2 2 -2 -2",
            "0 -7 -2 -2 -2 -2 2 -2 2 -2",
            "0 8 -2 -2 -2 -2 -2 2 -2 -2",
            "0 -7 -2 -2 -2 -2 2 -2 2 -2",
            "0 -3 0 0 0 0 0 0 0 0"
        };

        for (int j = 0; j < sizeY; j++)
        {
            var line = Console.ReadLine();
            //var line = roomsStr[j];
            Console.Error.WriteLine(line);

            var roomLine = line.Split(' ');
            for (var i = 0; i < sizeX; i++)
            {
                var roomType = int.Parse(roomLine[i]);
                map[sizeX * j + i] = Room.CreateRoom(Math.Abs(roomType), roomType > 0);
            }
        }

        var exitStr = Console.ReadLine();
        //var exitStr = "1";
        Console.Error.WriteLine(exitStr);
        exit = new Coord { X = int.Parse(exitStr), Y = sizeY - 1 };

        rooms = new Dictionary<int, Room>();
        for (var i = 0; i <= 13; i++)
        {
            rooms[i] = Room.CreateRoom(i, true);
        }
    }

    private static void GetParamUser(out Coord indyCoord, out Direction indyPosition, out List<Coord> rocks, out List<Direction> rocksDirection)
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        indyCoord = new Coord { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1]) };
        indyPosition = (Direction)Enum.Parse(typeof(Direction), inputs[2]);

        // Write an action using Console.WriteLine()
        Console.Error.WriteLine("XI:{0}, YI:{1}, POS:{2}", indyCoord.X, indyCoord.Y, indyPosition);

        int R = int.Parse(Console.ReadLine()); // the number of rocks currently in the grid.
        rocks = new List<Coord>();
        rocksDirection = new List<Direction>();

        for (int i = 0; i < R; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int XR = int.Parse(inputs[0]);
            int YR = int.Parse(inputs[1]);
            string POSR = inputs[2];
            var coord = new Coord { X = int.Parse(inputs[0]), Y = int.Parse(inputs[1]) };
            rocks.Add(coord);
            var position = (Direction)Enum.Parse(typeof(Direction), inputs[2]);
            rocksDirection.Add(position);
            Console.Error.WriteLine("XR:{0}, YR:{1}, POS:{2}", coord.X, coord.Y, position);
        }
    }

    private static void IndyVsRock(
        Coord indyCoord,
        Direction indyDirection,
        List<string> indyActions,
        List<Coord> rocksCoord,
        Dictionary<int, List<string>> rocksActions,
        out Coord coord,
        out int roomType)
    {
        List<Coord> indyCoordsAction;
        List<int> indyRoomsTypeAction;
        GetCoordAndTypeFromList(indyActions, out indyCoordsAction, out indyRoomsTypeAction);

        Coord coordToUnblock;
        int roomTypeToUnblock;
        int countToUnblock;
        bool unblockDone;
        GetCoordToUnBlock(indyCoord, indyCoordsAction, indyRoomsTypeAction, out coordToUnblock, out roomTypeToUnblock, out countToUnblock, out unblockDone);
        if (coordToUnblock != null)
        {
            Console.Error.WriteLine("indy unblock i:{0}, j:{1}, type:{2}, count:{3}, success:{4}", coordToUnblock.X, coordToUnblock.Y, roomTypeToUnblock, countToUnblock, unblockDone);
        }
        else
        {
            Console.Error.WriteLine("indy unblock i:{0}, j:{1}, type:{2}, count:{3}, success:{4}", -1, -1, roomTypeToUnblock, countToUnblock, unblockDone);
        }

        coord = coordToUnblock;
        roomType = roomTypeToUnblock;

        var count = int.MaxValue;
        var countImpact = int.MaxValue;
        var deepImpact = int.MaxValue;
        var rockIndex = 0;
        foreach (var roackActions in rocksActions.Values)
        {
            List<Coord> rockCoordsAction;
            List<int> rockRoomsTypeAction;
            GetCoordAndTypeFromList(roackActions, out rockCoordsAction, out rockRoomsTypeAction);

            Coord coordToBlock;
            int countToBlock;
            bool rockblocked;
            GetCoordToBlock(rocksCoord[rockIndex], rockCoordsAction, rockRoomsTypeAction, out coordToBlock, out countToBlock, out rockblocked);

            int countRockImpact;
            var hasImpact = GetRockImpactIndy(rockCoordsAction, indyCoordsAction, out countRockImpact);

            if(coordToBlock != null)
            {
                Console.Error.WriteLine("rock block i:{0}, j:{1}, count:{2}, deepImpact:{3}", coordToBlock.X, coordToBlock.Y, countToBlock, countRockImpact);
            }
            
            if (hasImpact && coordToBlock != null && coordToUnblock != null
            && coordToBlock.X == coordToUnblock.X && coordToBlock.Y == coordToUnblock.Y
            && countToBlock > 0)
            {
                count = countToBlock;
                coord = coordToBlock;
                deepImpact = countRockImpact;

                var iCoord = indyCoordsAction[countRockImpact - 1];
                var iDir = GetDirection(iCoord, coord);
                
                var room = Room.CreateRoom(roomTypeToUnblock, true);
                var way = GetWay(room, iDir);
                
                var rCoord = rockCoordsAction[countRockImpact - 1];
                var rDir = GetDirection(rCoord, coord);
                
                int roomTypeMin;
                int roomTypeMax;
                GetRoomTypeInterval(room, out roomTypeMin, out roomTypeMax);
                for (var i = roomTypeMin; i <= roomTypeMax; i++)
                {
                    var current = Room.CreateRoom(i, true);
                    if (current.Ways.Exists(w => w.In == way.In && w.Out == way.Out)
                        && !current.Ways.Exists(w => w.In == rDir))
                    {
                        roomType = i;
                        Console.Error.WriteLine("rock research:{0}", i);
                    }
                }
            }
            else if (hasImpact && coordToBlock != null && !rockblocked
            && (countRockImpact < countToUnblock || countToBlock < countToUnblock)
            && countRockImpact < deepImpact
            && countToBlock > 0)
            {
                deepImpact = countRockImpact;
                count = countToBlock;
                coord = coordToBlock;
                countImpact = countRockImpact;
                var room = GetRoomFromMap(coord);
                roomType = room.LeftType;
            }
        }

        if (coord != null)
        {
            Console.Error.WriteLine("instruction i:{0}, j:{1}, roomType:{2}", coord.X, coord.Y, roomType);
        }
    }

    private static bool CanBlockRockandUnblockIndy(Coord coordToUnblock, int roomTypeToUnblock, Coord coordToBlock, out int roomType)
    {
        if (coordToUnblock != null && coordToUnblock.X == coordToBlock.X && coordToUnblock.Y == coordToBlock.Y)
        {
            var room = GetRoomFromMap(coordToUnblock);
            roomType = roomTypeToUnblock;
            return true;
        }

        roomType = roomTypeToUnblock;
        return false;
    }

    private static bool GetRockImpactIndy(List<Coord> rockCoordsAction, List<Coord> indyCoordsAction, out int index)
    {
        index = 0;
        var success = false;
        foreach (var indyCoord in indyCoordsAction)
        {
            if (index < rockCoordsAction.Count)
            {
                var rockCoord = rockCoordsAction[index];
                if (rockCoord.X == indyCoord.X && rockCoord.Y == indyCoord.Y)
                {
                    success = true;
                    break;
                }
            }

            index++;
        }

        return success;
    }

    private static void GetCoordAndTypeFromList(List<string> actions, out List<Coord> coords, out List<int> roomTypes)
    {
        coords = new List<Coord>();
        roomTypes = new List<int>();

        foreach (var action in actions)
        {
            var split = action.Split(' ');
            coords.Add(new Coord { X = int.Parse(split[0]), Y = int.Parse(split[1]) });
            roomTypes.Add(int.Parse(split[2]));
        }
    }

    private static void SendOutput(Coord coord, int roomType)
    {
        if (coord == null)
        {
            Console.WriteLine("WAIT");
            return;
        }

        var room = map[sizeX * coord.Y + coord.X];
        var isRoom = room.RoomType == roomType;
        var isLeft = room.LeftType == roomType;
        var isRight = room.RightType == roomType;

        if (isRoom)
        {
            Console.WriteLine("WAIT");
        }
        else if (isLeft)
        {
            Console.WriteLine("{0} {1} {2}", coord.X, coord.Y, "LEFT");
            map[sizeX * coord.Y + coord.X] = rooms[room.LeftType];
        }
        else if (isRight)
        {
            Console.WriteLine("{0} {1} {2}", coord.X, coord.Y, "RIGHT");
            map[sizeX * coord.Y + coord.X] = rooms[room.RightType];
        }
        else
        {
            Console.WriteLine("{0} {1} {2}", coord.X, coord.Y, "LEFT");
            map[sizeX * coord.Y + coord.X] = rooms[room.LeftType];
        }
    }

    private static void GetCoordToBlock(Coord rock, List<Coord> coordsAction, List<int> roomsTypeAction, out Coord coordBlock, out int count, out bool alreadyBlock)
    {
        alreadyBlock = false;
        coordBlock = null;
        count = int.MaxValue;

        var index = 0;
        foreach (var coord in coordsAction)
        {
            if(index != 0)
            {
                var room = map[sizeX * coord.Y + coord.X];
                var roomType = roomsTypeAction[index];
                if (room.RoomType != roomType)
                {
                    coordBlock = coord;
                    alreadyBlock = true;
                    count = index;
                    break;
                }
    
                if (room.RoomType == roomType && room.CanRotate)
                {
                    coordBlock = coord;
                    alreadyBlock = false;
                    count = index;
                    break;
                }
            }

            index++;
        }
    }

    private static void GetCoordToUnBlock(Coord rock, List<Coord> coordsAction, List<int> roomsTypeAction, out Coord coordUnblock, out int roomTypeToUnblock, out int count, out bool alreadyUnblock)
    {
        alreadyUnblock = true;
        coordUnblock = null;
        count = int.MaxValue;
        roomTypeToUnblock = -1;

        var index = 0;
        foreach (var coord in coordsAction)
        {
            if(index != 0)
            {
                var roomType = roomsTypeAction[index];
                var room = map[sizeX * coord.Y + coord.X];
    
                if (room.RoomType != roomType && room.CanRotate)
                {
                    coordUnblock = coord;
                    roomTypeToUnblock = roomType;
                    alreadyUnblock = false;
                    count = index;
                    break;
                }
            }

            index++;
        }
    }

    private static bool FindWay(
        Coord coord,
        int roomType,
        bool canRotate,
        Direction direction,
        out List<string> actions)
    {
        var room = Room.CreateRoom(roomType, canRotate);
        var way = GetWay(room, direction);
        //Console.Error.WriteLine("{0} {1} {2} {3}", coord.X, coord.Y, room.RoomType, canRotate);

        if (!CanContinue(way, coord, room.RoomType))
        {
            actions = null;
            return false;
        }

        if (coord.X == exit.X && coord.Y == exit.Y)
        {
            //Console.Error.WriteLine("Success");
            actions = new List<string>();
            return true;
        }

        int minRoomType;
        int maxRoomType;
        var nextCoord = GetNextCoord(coord, way);
        var nextRoom = GetRoomFromMap(nextCoord);
        var nextDirection = Invert(way.Out);
        GetRoomTypeInterval(nextRoom, out minRoomType, out maxRoomType);

        for (int i = minRoomType; i <= maxRoomType; i++)
        {
            if (FindWay(nextCoord, i, nextRoom.CanRotate, nextDirection, out actions))
            {
                var action = string.Format("{0} {1} {2}", nextCoord.X, nextCoord.Y, i);
                //Console.Error.WriteLine(action);
                actions.Add(action);
                return true;
            }
        }

        actions = null;
        return false;
    }

    private static void GetRoomTypeInterval(Room room, out int minInterval, out int maxInterval)
    {
        minInterval = room.RoomType;
        maxInterval = room.RoomType;
        if (room.CanRotate)
        {
            for (int i = 0; i < room.NbType; i++)
            {
                room = Room.CreateRoom(room.LeftType, room.CanRotate);
                if (room.RoomType < minInterval)
                {
                    minInterval = room.RoomType;
                }
                if (room.RoomType > maxInterval)
                {
                    maxInterval = room.RoomType;
                }
            }
        }
    }

    private static Room GetRoomFromMap(Coord coord)
    {
        return map[sizeX * coord.Y + coord.X];
    }

    private static bool CanContinue(Way way, Coord coord, int roomType)
    {
        var result = way == null
             || way.Out == Direction.TOP
             || (way.Out == Direction.LEFT && coord.X == 0)
             || (way.Out == Direction.RIGHT && coord.X == sizeX - 1)
             || (way.Out == Direction.BOTTOM && coord.X != exit.X && coord.Y == exit.Y)
             || roomType == 0;
        return !result;
    }

    private static Coord GetNextCoord(Coord coord, Way way)
    {
        var x = coord.X;
        var y = coord.Y;

        switch (way.Out)
        {
            case Direction.TOP:
                y--;
                break;
            case Direction.LEFT:
                x--;
                break;
            case Direction.BOTTOM:
                y++;
                break;
            case Direction.RIGHT:
                x++;
                break;
        }

        return new Coord { X = x, Y = y };
    }

    private static Way GetWay(Room room, Direction direction)
    {
        return room.Ways.Find(w => w.In == direction);
    }

    private static Direction GetDirection(Coord from, Coord to)
    {
        var direction = Direction.TOP;
        if (from.X < to.X && from.Y == to.Y)
        {
            direction = Direction.LEFT;
        }
        if (from.X > to.X && from.Y == to.Y)
        {
            direction = Direction.RIGHT;
        }
        if (from.X == to.X && from.Y < to.Y)
        {
            direction = Direction.TOP;
        }
        if (from.X == to.X && from.Y > to.Y)
        {
            direction = Direction.BOTTOM;
        }

        return direction;
    }

    private static Direction Invert(Direction direction)
    {
        switch (direction)
        {
            case Direction.BOTTOM:
                return Direction.TOP;
            case Direction.LEFT:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.TOP:
                return Direction.BOTTOM;
            default:
                return Direction.TOP;
        }
    }
}

public class Room
{
    public int RoomType { get; private set; }
    public int LeftType { get; private set; }
    public int RightType { get; private set; }
    public int NbType { get; private set; }
    public bool CanRotate { get; private set; }
    public List<Way> Ways { get; private set; }

    public static Room CreateRoom(int roomType, bool canRotate)
    {
        var ways = new List<Way>();
        var left = 0;
        var right = 0;
        var nbType = 0;

        switch (roomType)
        {
            case 0:
                left = 0;
                right = 0;
                nbType = 1;
                break;
            case 1:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                left = 0;
                right = 0;
                nbType = 1;
                break;
            case 2:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.LEFT });
                left = 3;
                right = 3;
                nbType = 2;
                break;
            case 3:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                left = 2;
                right = 2;
                nbType = 2;
                break;
            case 4:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.LEFT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                left = 5;
                right = 5;
                nbType = 2;
                break;
            case 5:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                left = 4;
                right = 4;
                nbType = 2;
                break;
            case 6:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.RIGHT });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.LEFT });
                left = 9;
                right = 7;
                nbType = 4;
                break;
            case 7:
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                left = 6;
                right = 8;
                nbType = 4;
                break;
            case 8:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                left = 7;
                right = 9;
                nbType = 4;
                break;
            case 9:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                ways.Add(new Way { In = Direction.TOP, Out = Direction.BOTTOM });
                left = 8;
                right = 6;
                nbType = 4;
                break;
            case 10:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.LEFT });
                left = 13;
                right = 11;
                nbType = 4;
                break;
            case 11:
                ways.Add(new Way { In = Direction.TOP, Out = Direction.RIGHT });
                left = 10;
                right = 12;
                nbType = 4;
                break;
            case 12:
                ways.Add(new Way { In = Direction.RIGHT, Out = Direction.BOTTOM });
                left = 11;
                right = 13;
                nbType = 4;
                break;
            case 13:
                ways.Add(new Way { In = Direction.LEFT, Out = Direction.BOTTOM });
                left = 12;
                right = 10;
                nbType = 4;
                break;
        }
        return new Room
        {
            Ways = ways,
            RoomType = roomType,
            LeftType = left,
            RightType = right,
            NbType = nbType,
            CanRotate = canRotate
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

public class Coord
{
    public int X { get; set; }
    public int Y { get; set; }
}
