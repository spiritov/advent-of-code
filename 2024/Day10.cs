class Day10
{
    private readonly List<string> input = ReadInput.CreateListForDay(10);
    readonly List<List<int>> map = new();
    record struct Position(int Row, int Col);
    private readonly HashSet<Position> trailheads = new();
    private readonly List<Position> maxHeightPositions = new();
    private int max;

    private List<List<int>> CreateMap()
    {
        max = input.Count - 1;
        foreach (string s in input)
        {
            List<int> row = new();
            foreach (char c in s)
            {
                row.Add(c - '0');
            }
            map.Add(row);
        }
        return map;
    }

    private void FindTrailheads()
    {
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                if (map[i][j] == 0)
                {
                    trailheads.Add(new(i, j));
                }
            }
        }
    }

    private bool IsInBounds(Position pos)
    {
        return pos.Row >= 0 && pos.Row <= max && pos.Col >= 0 && pos.Col <= max;
    }

    private int FindTrail(Position pos, bool useAll)
    {
        int height = map[pos.Row][pos.Col];
        if (height == 9) //stop at 9
        {
            maxHeightPositions.Add(new Position(pos.Row, pos.Col));
        }
        var surroundingPositions = new HashSet<Position> { new(pos.Row - 1, pos.Col), new(pos.Row, pos.Col + 1), new(pos.Row + 1, pos.Col), new(pos.Row, pos.Col - 1) };  //NESW
        var validPositions = new HashSet<Position>();

        foreach (var position in surroundingPositions)
        {
            if (IsInBounds(position))
            {
                if (map[position.Row][position.Col] == height + 1) //if 1 greater
                {
                    validPositions.Add(position);
                }
            }
        }

        if (!useAll)
        {
            foreach (var position in validPositions)
            {
                FindTrail(position, false);
            }
            return maxHeightPositions.ToHashSet().Count;
        }
        else
        {
            foreach (var position in validPositions)
            {
                FindTrail(position, true);
            }
            return maxHeightPositions.Count;
        }
    }

    public void Part1()
    {
        int sum = 0;
        CreateMap();
        FindTrailheads();
        foreach (var trailhead in trailheads)
        {

            sum += FindTrail(trailhead, false);
            maxHeightPositions.Clear();
        }
        Console.WriteLine($"Part 1: {sum}");
    }

    public void Part2()
    {
        int sum = 0;
        foreach (var trailhead in trailheads)
        {

            sum += FindTrail(trailhead, true);
            maxHeightPositions.Clear();
        }
        Console.WriteLine($"Part 2: {sum}");
    }
}