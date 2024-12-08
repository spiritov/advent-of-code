using System.Diagnostics;

class Day6
{
    private readonly List<string> input = ReadInput.CreateListForDay(6);
    private (int, int) startPosition = (0, 0);
    private (int, int) currentPosition = (0, 0);
    private Direction direction = Direction.Up;
    private readonly HashSet<(int, int)> path = new(500);
    private readonly HashSet<(int, int)> obstructions = new(100);
    private int maxRow = 0;
    private int maxCol = 0;
    private bool solved = false;

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private bool CanMoveTo((int, int) position)
    {
        if (position.Item1 > -1 && position.Item1 < maxRow && position.Item2 > -1 && position.Item2 < maxCol)
        {
            return !obstructions.Contains(position);
        }
        else //outside bounds
        {
            solved = true;
            return false;
        }
    }

    private void MoveStep((int, int) position, bool addToPath)
    {
        (int, int) testPosition = position;
        switch (direction)
        {
            case Direction.Up:
                testPosition.Item1--;
                break;
            case Direction.Right:
                testPosition.Item2++;
                break;
            case Direction.Down:
                testPosition.Item1++;
                break;
            case Direction.Left:
                testPosition.Item2--;
                break;
            default:
                Console.WriteLine("invalid direction");
                break;
        }
        if (CanMoveTo(testPosition))
        {
            currentPosition = testPosition;
            if (addToPath)
            {
                path.Add(currentPosition);
            }
        }
        else
        {
            direction = (direction == Direction.Left) ? 0 : (Direction)(Array.IndexOf(Enum.GetValues(direction.GetType()), direction) + 1); //wrap around enum
        }
    }

    private void InitializeMap()
    {
        maxRow = input.Count;
        maxCol = input[0].Length;
        for (int i = 0; i < maxRow; i++)
        {
            if (input[i].Contains('^')) //find starting guard position
            {
                startPosition = (i, input[i].IndexOf('^'));
                currentPosition = startPosition;
            }

            int searchIndex = 0;
            while (searchIndex != -1) //find all obstruction positions
            {
                searchIndex = input[i].IndexOf('#', searchIndex);
                if (searchIndex != -1)
                {
                    obstructions.Add((i, searchIndex));
                    searchIndex++;
                }
            }
        }
    }

    public void Part1()
    {
        InitializeMap();
        path.Add(currentPosition);

        while (!solved)
        {
            MoveStep(currentPosition, true);
        }

        Console.WriteLine($"Part 1: {path.Count}");
    }

    public void Part2()
    {
        int loops = 0;
        foreach ((int, int) location in path) //i represents each step along the path
        {
            HashSet<((int, int) Position, Direction Dir)> stepsWithDirection = new();
            solved = false;
            direction = Direction.Up;

            obstructions.Add(location); //remove after iterating
            currentPosition = startPosition;

            while (!solved)
            {
                MoveStep(currentPosition, false); //we're tracking with direction, for Part2
                if (!stepsWithDirection.Add(new(currentPosition, direction)))
                {
                    loops++;
                    break;
                }
            }
            obstructions.Remove(location);
        }

        Console.WriteLine($"Part 2: {loops}");
    }
}