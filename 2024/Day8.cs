using System.Diagnostics;
using System.Numerics;

class Day8
{
    private readonly List<string> input = ReadInput.CreateListForDay(8);

    record struct Position(int Row, int Col)
    {
        public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);
        public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);
    }

    private readonly Dictionary<char, List<Position>> nodeGroups = new(); //node character, and list of locations
    private readonly HashSet<Position> antinodeLocations = new();
    private int maxRow = 0;
    private int maxCol = 0;

    private void PopulateNodeGroups()
    {
        for (int i = 0; i < maxRow; i++)
        {
            for (int j = 0; j < maxCol; j++)
            {
                char iteratedChar = input[i][j];
                if (char.IsAsciiLetterOrDigit(iteratedChar))
                {
                    if (!nodeGroups.TryGetValue(iteratedChar, out var positions))
                    {
                        nodeGroups.Add(iteratedChar, new List<Position> { new(i, j) });
                    }
                    else
                    {
                        positions.Add(new(i, j));
                    }
                }
            }
        }
    }

    private bool IsInBounds(Position p)
    {
        return p.Row > -1 && p.Row < maxRow && p.Col > -1 && p.Col < maxCol;
    }
    private void FindAntinodes(bool includeResonantFrequencies)
    {
        foreach (var (_, positions) in nodeGroups)
        {
            for (int i = 0; i < positions.Count - 1; i++)
            {
                for (int j = 1; j < positions.Count - i; j++)
                {
                    Position nodeA = positions[i];
                    Position nodeB = positions[i + j];
                    var delta = nodeA - nodeB;
                    var antinodeAB = nodeA + delta;
                    var antinodeBA = nodeB - delta;
                    if (IsInBounds(antinodeAB))
                    {
                        antinodeLocations.Add(antinodeAB);
                    }
                    if (IsInBounds(antinodeBA))
                    {
                        antinodeLocations.Add(antinodeBA);
                    }

                    if (includeResonantFrequencies)
                    {
                        antinodeLocations.Add(nodeA);
                        antinodeLocations.Add(nodeB);

                        var previousAntinode = antinodeAB + delta;
                        while (IsInBounds(previousAntinode))
                        {
                            antinodeLocations.Add(previousAntinode);
                            previousAntinode += delta;
                        }

                        var nextAntinode = antinodeBA - delta;
                        while (IsInBounds(nextAntinode))
                        {
                            antinodeLocations.Add(nextAntinode);
                            nextAntinode -= delta;
                        }
                    }
                }
            }
        }
    }

    public void Part1()
    {
        maxRow = input.Count;
        maxCol = input[0].Length;
        PopulateNodeGroups();

        FindAntinodes(false);
        Console.WriteLine($"Part 1: {antinodeLocations.Count}");
    }
    public void Part2()
    {
        FindAntinodes(true);
        Console.WriteLine($"Part 2: {antinodeLocations.Count}");
    }
}