class Day11
{
    private readonly string input = ReadInput.CreateListForDay(11)[0];
    private List<long> stones = new();
    private Dictionary<(long value, int cycle), long> stonesCache = new();
    int maxBlinks;

    private List<long> SetStones()
    {
        return input.Split(' ').Select(long.Parse).ToList();
    }

    private int Digits(long num)
    {
        int digits = 1;
        while (num / 10 != 0)
        {
            digits++;
            num /= 10;
        }
        return digits;
    }

    private long Blink(long value, int cycle)
    {
        if (cycle == maxBlinks)
        {
            return 1;
        }
        if (stonesCache.TryGetValue((value, cycle), out long resultingStones)) //we can add to cache safely if this fails
        {
            return resultingStones;
        }
        if (value == 0)
        {
            var zeroCycle = Blink(1, cycle + 1);
            stonesCache.Add((value, cycle), zeroCycle);
            return zeroCycle;
        }
        int digits = Digits(value);
        if (digits % 2 == 0) //even number of digits
        {
            var evenCycle = Blink((long)(value / Math.Pow(10, digits / 2)), cycle + 1) + Blink((long)(value % Math.Pow(10, digits / 2)), cycle + 1);
            stonesCache.Add((value, cycle), evenCycle);
            return evenCycle;
        }
        var multCycle = Blink(value * 2024, cycle + 1);
        stonesCache.Add((value, cycle), multCycle);
        return multCycle;
    }

    public void Part1()
    {
        long sum = 0;
        maxBlinks = 25;
        stones = SetStones();
        foreach (var stone in stones)
        {
            sum += Blink(stone, 0);
        }
        Console.WriteLine($"Part 1: {sum}");
    }

    public void Part2()
    {
        long sum = 0;
        maxBlinks = 75;
        stones = SetStones();
        stonesCache.Clear();
        foreach (var stone in stones)
        {
            sum += Blink(stone, 0);
        }
        Console.WriteLine($"Part 2: {sum}");
    }
}