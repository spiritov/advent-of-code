class Day12
{
    private readonly List<string> input = ReadInput.CreateListForDay(12);
    record struct Position(int Row, int Col);
    readonly List<List<char>> map = new();
    readonly List<List<Position>> regions = new();
    readonly List<int> regionPerimeters = new();
    readonly List<int> regionSides = new();
    int maxBound;

    private static List<Position> SortPositions(List<Position> positions) //will only have 3 positions
    {
        if (positions[0].Row == positions[2].Row || positions[0].Col == positions[2].Col)
        {
            return positions;
        }
        //swap middle element with whichever position it doesn't share a row / col
        return (positions[0].Row == positions[1].Row || positions[0].Col == positions[1].Col)
        ? new() { positions[0], positions[2], positions[1] }
        : new() { positions[1], positions[0], positions[2] };
    }

    private bool IsInBounds(Position pos)
    {
        return pos.Row >= 0 && pos.Row <= maxBound && pos.Col >= 0 && pos.Col <= maxBound;
    }

    private void PopulateRegion(int row, int col, char letter)
    {
        regions.Last().Add(new Position(row, col));
        var neighboringPositions = new List<Position> { new(row - 1, col), new(row, col + 1), new(row + 1, col), new(row, col - 1) };  //NESW
        var cornerPositions = new List<Position> { new(row - 1, col - 1), new(row - 1, col + 1), new(row + 1, col + 1), new(row + 1, col - 1) };  //NW, NE, SE, SW
        var matchingPositions = new List<Position>();

        foreach (var neighbor in neighboringPositions)
        {
            if (IsInBounds(neighbor) && map[neighbor.Row][neighbor.Col] == letter)
            {
                matchingPositions.Add(neighbor);
            }
        }
        foreach (var match in matchingPositions)
        {
            if (!regions.Last().Contains(match)) //if position doesn't exist, add it
            {
                PopulateRegion(match.Row, match.Col, letter);
            }
        }
        regionPerimeters[^1] += 4 - matchingPositions.Count; //number of visible sides for current position

        int corners = 0; //part 2
        switch (matchingPositions.Count)
        {
            case 0: //always 4 corners
                corners = 4;
                break;
            case 1: //always 2 corners
                corners = 2;
                break;
            case 2: //0 corners, unless neighbors are perpendicular
                if (matchingPositions[0].Row == matchingPositions[1].Row || matchingPositions[0].Col == matchingPositions[1].Col)
                {
                    corners = 0;
                }
                else //neighbors are perpendicular
                {
                    corners = 2;
                    Position cornerA = new(matchingPositions[0].Row, matchingPositions[1].Col);
                    Position cornerB = new(matchingPositions[1].Row, matchingPositions[0].Col);
                    if (map[cornerA.Row][cornerA.Col] == letter && map[cornerB.Row][cornerB.Col] == letter) //one of these is always our current position
                    {
                        corners = 1;
                    }
                }
                break;
            case 3:
                corners = 2;
                matchingPositions = SortPositions(matchingPositions);
                Position cornerC = new(matchingPositions[0].Row, matchingPositions[1].Col);
                Position cornerD = new(matchingPositions[1].Row, matchingPositions[0].Col);
                Position cornerE = new(matchingPositions[1].Row, matchingPositions[2].Col);
                Position cornerF = new(matchingPositions[2].Row, matchingPositions[1].Col);
                HashSet<Position> TwoCorners = new() { cornerC, cornerD, cornerE, cornerF }; //remove duplicate
                int occupied = 0;
                foreach (var cornerNeighbor in TwoCorners)
                {
                    if (map[cornerNeighbor.Row][cornerNeighbor.Col] == letter) //one of these is always our current position
                    {
                        occupied++;
                    }
                }
                corners -= occupied - 1; //to offset counting our current position
                break;
            case 4: //4 corners, but no corner for each corner neighbor that exists
                corners = 4;
                foreach (var cornerNeighbor in cornerPositions)
                {
                    if (map[cornerNeighbor.Row][cornerNeighbor.Col] == letter)
                    {
                        corners--;
                    }
                }
                break;
            default:
                break;
        }
        regionSides[^1] += corners;
    }

    private void FindRegions()
    {
        for (int row = 0; row <= maxBound; row++)
        {
            for (int col = 0; col <= maxBound; col++)
            {
                char foundLetter = map[row][col];
                if (foundLetter != '.')
                {
                    regions.Add(new());
                    regionPerimeters.Add(0);
                    regionSides.Add(0); //part 2
                    PopulateRegion(row, col, foundLetter);

                    foreach (var position in regions.Last())
                    {
                        map[position.Row][position.Col] = '.'; //region finished
                    }
                }
            }
        }
    }

    private int GetPrice(bool bulk)
    {
        int total = 0;
        for (int i = 0; i < regions.Count; i++)
        {
            total += bulk ? regions[i].Count * regionSides[i] : regions[i].Count * regionPerimeters[i];
        }
        return total;
    }

    private void InputToGrid()
    {
        foreach (string s in input)
        {
            map.Add(new());
            var currentGrid = map.Last();
            foreach (char c in s)
            {
                currentGrid.Add(c);
            }
        }
    }

    public void Part1()
    {
        InputToGrid();
        maxBound = map.Count - 1;
        FindRegions();
        int price = GetPrice(false);
        Console.WriteLine($"Part 1: {price}");
    }

    public void Part2()
    {
        int price = GetPrice(true);
        Console.WriteLine($"Part 2: {price}");
    }

    private void PrintMap() //debug
    {
        for (int row = 0; row <= maxBound; row++)
        {
            for (int col = 0; col <= maxBound; col++)
            {
                Console.Write(map[row][col]);
            }
            Console.WriteLine();
        }
    }
}