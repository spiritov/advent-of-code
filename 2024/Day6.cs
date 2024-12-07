class Day6
{
    private readonly List<string> input = ReadInput.CreateListForDay(6);
    private (int, int) currentPosition = (0, 0);
    private Direction direction = Direction.Up;
    private List<(int, int)> path = new(500);
    private List<(int, int)> obstructions = new(100);
    private int maxSteps = 0;
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
            if (!obstructions.Contains(position))
            {
                return true;
            }
        }
        else //outside bounds
        {
            solved = true;
        }
        return false;
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

    private void InitializeLocations()
    {
        for (int i = 0; i < maxRow; i++)
        {
            if (input[i].IndexOf('^') != -1) //find starting guard position
            {
                currentPosition = (i, input[i].IndexOf('^'));
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
        maxRow = input.Count;
        maxCol = input[0].Length;

        InitializeLocations();
        path.Add(currentPosition);

        solved = false;

        while (!solved)
        {
            MoveStep(currentPosition, true);
        }
        path = path.Distinct().ToList();
        maxSteps = path.Count;
        Console.WriteLine($"Part 1: {path.Count}");
    }

    public void Part2()
    {
        int loops = 0;

        for (int i = 1; i < maxSteps; i++) //i represents each step along the path
        {
            int steps = 0;
            bool looping = false;
            solved = false;
            direction = Direction.Up;

            obstructions.Add(path[i]); //remove after iterating
            currentPosition = path[0];

            while (!solved && !looping)
            {
                MoveStep(currentPosition, false); //we're tracking steps, not path, for Part2
                steps++;
                if (steps > maxSteps * 1.5) //if we've hit this many steps, we're probably looping
                {
                    looping = true;
                    loops++;
                }
            }
            obstructions.RemoveAt(obstructions.Count - 1);
        }

        Console.WriteLine($"Part 2: {loops}");
    }
}