using System.Linq.Expressions;
using System.Text.RegularExpressions;

class Day3
{
    private readonly List<string> _input = ReadInput.CreateListForDay(3);
    Regex mulPattern = new Regex(@"mul\(\d+,\d+\)");
    Regex intPattern = new Regex(@"\d+");

    public void Part1()
    {
        int sum = 0;
        foreach (string line in _input)
        {
            foreach (Match m in mulPattern.Matches(line))
            {
                sum += int.Parse(intPattern.Matches(m.Value)[0].Value) * int.Parse(intPattern.Matches(m.Value)[1].Value);
            }
        }
        Console.WriteLine(sum);
    }



    public void Part2()
    {
        int sum = 0;
        Regex allPatterns = new Regex(@"mul\(\d+,\d+\)|do\(\)|don't\(\)");

        foreach (string line in _input)
        {
            MatchCollection lineMatches = allPatterns.Matches(line);
            bool enable = true;

            foreach (Match m in lineMatches)
            {
                if (m.Value == "do()")
                {
                    enable = true;
                }
                else if (m.Value == "don't()")
                {
                    enable = false;
                }
                else if (enable)
                {
                    sum += int.Parse(intPattern.Matches(m.Value)[0].Value) * int.Parse(intPattern.Matches(m.Value)[1].Value);
                }
            }
        }
        Console.WriteLine(sum);
    }

    /* original answer, works
    public void Part2()
    {
        int sum = 0;
        Regex enablePattern = new Regex(@"do\(\)|don't\(\)");

        foreach (string line in _input)
        {
            MatchCollection lineMatches = mulPattern.Matches(line);
            MatchCollection enableMatches = enablePattern.Matches(line);

            foreach (Match m in lineMatches)
            {
                string lastInstruction = "do()";
                foreach (Match n in enableMatches)
                {
                    if (n.Index < m.Index)
                    {
                        lastInstruction = n.Value;
                    }
                }
                if (lastInstruction == "do()")
                {
                    sum += int.Parse(intPattern.Matches(m.Value)[0].Value) * int.Parse(intPattern.Matches(m.Value)[1].Value);
                }
            }
        }
        Console.WriteLine(sum);
    } */
}