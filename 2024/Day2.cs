class Day2
{
    private readonly List<string> _input = ReadInput.CreateListForDay(2);

    private bool CheckIfSafe(List<int> report)
    {
        bool isIncreasing = report[1] > report[0]; //initial check
        bool isSafe = true;

        for (int i = 0; i < report.Count - 1; i++)
        {
            if (isIncreasing)
            {
                if (report[i + 1] - report[i] <= 0 || report[i + 1] - report[i] > 3)
                {
                    isSafe = false;
                }
            }
            else //decreasing
            {
                if (report[i] - report[i + 1] <= 0 || report[i] - report[i + 1] > 3)
                {
                    isSafe = false;
                }
            }
        }
        return isSafe;
    }

    public void Part1()
    {
        int safe = 0;
        foreach (string s in _input)
        {
            List<int> levels = s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (CheckIfSafe(levels))
            {
                safe++;
            }
        }

        Console.WriteLine(safe);

    }

    public void Part2()
    {
        int safe = 0;
        foreach (string s in _input)
        {
            List<int> levels = s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            if (CheckIfSafe(levels))
            {
                safe++;
            }
            else
            {
                for (int i = 0; i < levels.Count; i++)
                {
                    List<int> tempReport = new(levels);
                    tempReport.RemoveAt(i);
                    if (CheckIfSafe(tempReport))
                    {
                        safe++;
                        break;
                    }
                }
            }
        }
        Console.WriteLine(safe);
    }
}