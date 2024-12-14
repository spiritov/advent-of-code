class Day7
{
    private readonly List<string> input = ReadInput.CreateListForDay(7);
    private readonly HashSet<(long, List<long>)> equations = new(850);
    bool withConcat = false;

    private bool IsValid((long, List<long>) equation)
    {
        var testValue = equation.Item1;
        var operants = equation.Item2;
        if (operants.Count == 1) //exit condition
        {
            return testValue == operants[0];
        }

        //start from last index
        long last = operants.Last();
        int count = operants.Count;
        if (testValue % last == 0 && IsValid((testValue / last, operants.GetRange(0, count - 1))))
        {
            return true;
        }
        if (testValue > last && IsValid((testValue - last, operants.GetRange(0, count - 1))))
        {
            return true;
        }
        if (withConcat && testValue > last)
        {
            bool canConcat = last.ToString() == testValue.ToString()[^last.ToString().Length..]; //see if last can concat into testValue
            if (canConcat && IsValid((long.Parse(testValue.ToString()[..^last.ToString().Length]), operants.GetRange(0, count - 1)))) //split testValue
            {
                return true;
            }
        }
        return false;
    }

    private void CreateEquations()
    {
        foreach (string s in input)
        {
            int colonPosition = s.IndexOf(':');
            long bufferKey = long.Parse(s.Substring(0, colonPosition));
            List<long> bufferValue = s.Substring(colonPosition + 2).Split(' ').Select(long.Parse).ToList();

            equations.Add(new(bufferKey, bufferValue));
        }
    }

    public void Part1()
    {
        long sum = 0;
        CreateEquations();
        foreach (var equation in equations)
        {
            if (IsValid(equation))
            {
                sum += equation.Item1;
            }
        }
        Console.WriteLine($"Part 1: {sum}");
    }

    public void Part2()
    {
        withConcat = true;
        long sum = 0;
        foreach (var equation in equations)
        {
            if (IsValid(equation))
            {
                sum += equation.Item1;
            }
        }
        Console.WriteLine($"Part 2: {sum}");
    }
}