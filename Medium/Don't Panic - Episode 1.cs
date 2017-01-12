using System;
using System.Collections.Generic;

/**
 * Auto-generated code below aims at helping you parse
 * the standard input according to the problem statement.
 **/
class Player
{
    private static Dictionary<int, Gateway> gateways;
    private static readonly List<string> positionCovered = new List<string>();
    private static int startPosition = -1;

    static void Main(string[] args)
    {
        SetParameter();

        // game loop
        while (true)
        {
            var inputs = Console.ReadLine().Split(' ');
            var cloneFloor = int.Parse(inputs[0]); // floor of the leading clone
            var clonePos = int.Parse(inputs[1]); // position of the leading clone on its floor
            string direction = inputs[2]; // direction of the leading clone: LEFT or RIGHT

            // Write an action using Console.WriteLine()
            var action = ChooseAction(cloneFloor, clonePos, direction);

            Console.WriteLine(action); // action: WAIT or BLOCK
        }
    }

    private static string ChooseAction(int cloneFloor, int clonePos, string direction)
    {
        if (startPosition == -1)
        {
            startPosition = clonePos;
        }
        var action = string.Empty;
        if (cloneFloor == -1 && clonePos == -1 && direction == "NONE")
        {
            return "WAIT";
        }
        var vector = direction == "RIGHT" ? 1 : -1;
        
        var exit = gateways[cloneFloor].Position;
        Console.Error.WriteLine("floor:{0}, position:{1}, direction:{2}->{3}, exit:{4}", 
            cloneFloor, 
            clonePos, 
            direction, 
            vector, 
            exit);

        Console.Error.WriteLine("case1:{0}, case2:{1}, case3:{2}, case3:{3}", 
            startPosition == clonePos,
            (vector > 0 && clonePos <= exit),
            (vector < 0 && clonePos >= exit),
            positionCovered.Contains(cloneFloor + "-" + clonePos));
        
        if (startPosition == clonePos
            || (vector > 0 && clonePos <= exit)
            || (vector < 0 && clonePos >= exit)
            || positionCovered.Contains(cloneFloor + "-" + clonePos))
        {
            action = "WAIT";
        }
        else
        {
            action = "BLOCK";
            positionCovered.Add(cloneFloor + "-" + clonePos);
            Console.Error.WriteLine("position blocked Floor:{0}, Position:{1}", cloneFloor, clonePos);
        }
        return action;
    }

    private static void SetParameter()
    {
        string[] inputs;
        inputs = Console.ReadLine().Split(' ');
        var floors = int.Parse(inputs[0]); // number of floors
        var width = int.Parse(inputs[1]); // width of the area
        var rounds = int.Parse(inputs[2]); // maximum number of rounds
        var exitFloor = int.Parse(inputs[3]); // floor on which the exit is found
        var exitPos = int.Parse(inputs[4]); // position of the exit on its floor
        var totalClones = int.Parse(inputs[5]); // number of generated clones
        var nbAdditionalElevators = int.Parse(inputs[6]); // ignore (always zero)
        var elevators1 = int.Parse(inputs[7]); // number of elevators
        var elevators = new Dictionary<int, Gateway>
        {
            {exitFloor, new Gateway(exitFloor, exitPos, true)}
        };
        for (var i = 0; i < elevators1; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            var elevatorFloor = int.Parse(inputs[0]); // floor on which this elevator is found
            var elevatorPos = int.Parse(inputs[1]); // position of the elevator on its floor
            elevators.Add(elevatorFloor, new Gateway(elevatorFloor, elevatorPos));
        }

        SetParameter(floors, width, rounds, totalClones, elevators1, elevators);
    }

    private static void SetParameter(int floors, int width, int rounds, int totalClones, int elevators, Dictionary<int, Gateway> gatewayList)
    {
        gateways = gatewayList;
    }
}

internal class Gateway
{
    private int floor;
    private int position;
    private bool lastFloor;

    public Gateway(int elevatorFloor, int elevatorPos, bool lastFLoor = false)
    {
        this.floor = elevatorFloor;
        this.position = elevatorPos;
        this.lastFloor = lastFLoor;
    }

    public bool LastFloor
    {
        get { return lastFloor; }
    }

    public int Position
    {
        get { return position; }
    }
}
