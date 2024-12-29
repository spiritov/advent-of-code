class Day13
{
    private readonly List<string> input = ReadInput.CreateListForDay(13);
    record struct Position(double X, double Y)
    {
        public static Position operator +(Position a, Position b) => new(a.X + b.X, a.Y + b.Y);
        public static Position operator -(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    }
    record struct Machine(Position A, Position B, Position P);
    private readonly List<Machine> machines = new();

    private static double GetTokens(Machine m, bool Part2)
    {
        if (Part2)
            m.P += new Position(10000000000000, 10000000000000);

        //from solving A.X * APresses + B.X * BPresses = P.X and same equation for Y 
        var BPresses = ((m.P.X * m.A.Y) - (m.P.Y * m.A.X)) / ((m.B.X * m.A.Y) - (m.B.Y * m.A.X));
        var APresses = (m.P.Y - (m.B.Y * BPresses)) / m.A.Y;

        return APresses % 1 == 0 && BPresses % 1 == 0 ? (APresses * 3) + BPresses : 0;
    }

    private void PopulateMachines()
    {
        var tempMachine = new Machine();
        foreach (var s in input)
        {
            if (s.Contains('A'))
            {
                tempMachine.A = new(int.Parse(s.Substring(12, 2)), int.Parse(s.Substring(18, 2)));
            }
            else if (s.Contains('B'))
            {
                tempMachine.B = new(int.Parse(s.Substring(12, 2)), int.Parse(s.Substring(18, 2)));
            }
            else if (s.Contains('P'))
            {
                tempMachine.P = new(int.Parse(s[9..s.IndexOf(',')]), int.Parse(s[(s.IndexOf('Y') + 2)..]));
                machines.Add(tempMachine);
            }
        }
    }

    public void Part1()
    {
        double sum = 0;
        PopulateMachines();
        foreach (var m in machines)
        {
            sum += GetTokens(m, false);
        }
        Console.WriteLine($"Part 1: {sum}");
    }

    public void Part2()
    {
        long sum = 0;
        foreach (var m in machines)
        {
            sum += (long)GetTokens(m, true);
        }
        Console.WriteLine($"Part 2: {sum}");
    }
}