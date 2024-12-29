using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

class Day14
{
    private readonly List<string> input = ReadInput.CreateListForDay(14);
    record struct Position(int X, int Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    }
    private Position bounds = new(101, 103); //bounds of space
    private readonly List<Position> positions = new();
    private readonly List<Position> velocities = new();
    readonly Regex intPattern = new(@"-?\d+");

    private int GetSafetyFactor()
    {
        int Q1 = 0;
        int Q2 = 0;
        int Q3 = 0;
        int Q4 = 0;
        foreach (var p in positions)
        {
            if (p.X != bounds.X / 2 && p.Y != bounds.Y / 2)
            {
                if (p.X < bounds.X / 2 && p.Y < bounds.Y / 2)
                {
                    Q1++;
                }
                else if (p.X > bounds.X / 2 && p.Y < bounds.Y / 2)
                {
                    Q2++;
                }
                else if (p.X < bounds.X / 2 && p.Y > bounds.Y / 2)
                {
                    Q3++;
                }
                else Q4++;
            }
        }
        return Q1 * Q2 * Q3 * Q4;
    }

    private void ElapseSeconds(int seconds, bool Part2)
    {
        for (int i = 0; i < seconds; i++)
        {
            for (int j = 0; j < positions.Count; j++)
            {
                positions[j] += velocities[j];
                if (positions[j].X >= bounds.X)
                {
                    positions[j] = new(positions[j].X - bounds.X, positions[j].Y);
                }
                else if (positions[j].X < 0)
                {
                    positions[j] = new(bounds.X + positions[j].X, positions[j].Y);
                }
                if (positions[j].Y >= bounds.Y)
                {
                    positions[j] = new(positions[j].X, positions[j].Y - bounds.Y);
                }
                else if (positions[j].Y < 0)
                {
                    positions[j] = new(positions[j].X, bounds.Y + positions[j].Y);
                }
            }
            if (Part2)
                TryFindTree(i + 1);
        }
    }

    private void TryFindTree(int seconds)
    {
        bool probableTree = false;
        List<string> strings = new();
        for (int i = 0; i < bounds.Y; i++)
        {
            var emptyRow = string.Concat(Enumerable.Repeat(' ', bounds.X));
            strings.Add(emptyRow);
        }
        foreach (var p in positions)
        {
            StringBuilder sbRow = new(strings[p.Y]);
            sbRow[p.X] = '1';
            strings[p.Y] = sbRow.ToString();
        }
        foreach (var str in strings)
        {
            if (str.Contains("1111111111"))
            {
                probableTree = true;
            }
        }
        if (probableTree)
        {
            Console.WriteLine($"Possible tree at {seconds}");
        }
    }

    private void PopulateRobots()
    {
        foreach (string s in input)
        {
            var matches = intPattern.Matches(s);
            positions.Add(new(int.Parse(matches[0].Value), int.Parse(matches[1].Value)));
            velocities.Add(new(int.Parse(matches[2].Value), int.Parse(matches[3].Value)));
        }
    }

    public void Part1()
    {
        PopulateRobots();
        ElapseSeconds(100, false);
        Console.WriteLine($"Part 1: {GetSafetyFactor()}");
    }

    public void Part2()
    {
        positions.Clear();
        velocities.Clear();
        PopulateRobots();
        Console.Write($"Part 2: ");
        ElapseSeconds(10000, true); //magic number
    }
}