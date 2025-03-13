using System.Text;

class Day15
{
    private readonly List<string> input = ReadInput.CreateListForDay(15);
    private readonly List<List<char>> warehouse = new();
    private readonly List<List<char>> wideWarehouse = new();
    private string moves = "";
    private Position robotPosition = new();
    private readonly Dictionary<char, Position> moveVectors = new() {
        { '^', new(-1, 0)},
        { 'v', new(1, 0)},
        { '<', new (0, -1)},
        { '>', new (0, 1)}
    };
    private readonly Dictionary<char, (char L, char R)> wideSymbols = new() {
        { '@', ('@','.')},
        { 'O', ('[', ']')},
        { '.', ('.','.')},
        { '#', ('#','#')}
    };

    record struct Position(int Row, int Col)
    {
        public static Position operator +(Position a, Position b) => new(a.Row + b.Row, a.Col + b.Col);
        public static Position operator -(Position a, Position b) => new(a.Row - b.Row, a.Col - b.Col);
    }

    private void SetWideWarehouse()
    {
        int width = input[0].Length;
        for (int row = 0; row < input.Count; row++)
        {
            if (input[row] == "")
            {
                for (int r = 0; r < wideWarehouse.Count; r++)
                {
                    for (int c = 0; c < wideWarehouse[r].Count; c++)
                    {
                        if (wideWarehouse[r][c] == '@')
                        {
                            robotPosition = new(r, c);
                        }
                    }
                }
                PrintWarehouse(wideWarehouse);
                return;
            }

            List<char> currentRow = new();
            for (int col = 0; col < width; col++)
            {
                currentRow.Add(wideSymbols[input[row][col]].L);
                currentRow.Add(wideSymbols[input[row][col]].R);
            }
            wideWarehouse.Add(currentRow);
        }
    }

    private void SetWarehouse()
    {
        int width = input[0].Length;
        for (int row = 0; row < input.Count; row++)
        {
            if (input[row] == "")
            {
                SetMoves(row + 1);
                return; //exit when map is parsed
            }

            List<char> currentRow = new();
            for (int col = 0; col < width; col++)
            {
                if (input[row][col] == '@')
                {
                    robotPosition = new(row, col);
                }
                currentRow.Add(input[row][col]);
            }
            warehouse.Add(currentRow);
        }
    }

    private void SetMoves(int sequenceIndex)
    {
        StringBuilder sbMoves = new();
        for (int row = sequenceIndex; row < input.Count; row++)
        {
            sbMoves.Append(input[row]);
        }
        moves = sbMoves.ToString();
    }

    private void AttemptMove(char move, List<List<char>> warehouse)
    {
        Position attemptedPosition = robotPosition + moveVectors[move];

        Dictionary<Position, char> movingBoxes = new() { { robotPosition, '@' } };
        Position nextCheckedPosition = robotPosition + moveVectors[move];

        switch (warehouse[attemptedPosition.Row][attemptedPosition.Col])
        {
            case '.':
                warehouse[robotPosition.Row][robotPosition.Col] = '.';
                robotPosition = attemptedPosition;
                warehouse[robotPosition.Row][robotPosition.Col] = '@';
                break;
            case '#':
                break;
            case 'O':
                while (warehouse[nextCheckedPosition.Row][nextCheckedPosition.Col] == 'O')
                {
                    movingBoxes.Add(nextCheckedPosition, 'O');
                    nextCheckedPosition += moveVectors[move];
                }

                if (warehouse[nextCheckedPosition.Row][nextCheckedPosition.Col] == '.')
                {
                    foreach (var (box, _) in movingBoxes)
                    {
                        warehouse[box.Row][box.Col] = '.';
                    }

                    foreach (var (box, c) in movingBoxes)
                    {
                        warehouse[(box + moveVectors[move]).Row][(box + moveVectors[move]).Col] = c;
                        if (c == '@')
                        {
                            robotPosition += moveVectors[move];
                        }
                    }
                }
                break;
        }
    }

    public void Part1()
    {
        SetWarehouse();
        foreach (var move in moves)
        {
            AttemptMove(move, warehouse);
        }

        int sum = 0;
        for (int row = 0; row < warehouse.Count; row++)
        {
            for (int c = 0; c < warehouse[row].Count; c++)
            {
                if (warehouse[row][c] == 'O')
                {
                    sum += (row * 100) + c;
                }
            }
        }
        Console.WriteLine($"Part 1: {sum}");
    }

    private void PrintWarehouse(List<List<char>> warehouse)
    {
        foreach (var row in warehouse)
        {
            foreach (char c in row)
            {
                Console.Write(c);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

