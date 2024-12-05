using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

class Day1
{
    private readonly List<string> _input = ReadInput.CreateListForDay(1);
    private readonly List<int> _leftList = new();
    private readonly List<int> _rightList = new();

    private void PopulateLists(List<string> lists)
    {
        if (_leftList.Capacity == 0) //don't populate twice
        {
            foreach (string s in lists)
            {
                _leftList.Add(int.Parse(s.Substring(0, 5)));
                _rightList.Add(int.Parse(s.Substring(8, 5)));
                _leftList.Sort();
                _rightList.Sort();
            }
        }
    }

    public void Part1()
    {
        int sum = 0;
        PopulateLists(_input);

        for (int i = 0; i < _leftList.Count; i++)
        {
            int difference = _leftList[i] - _rightList[i];
            sum += Math.Abs(difference);
        }
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        int sum = 0;
        PopulateLists(_input);

        Dictionary<int, int> occurancesOf = new();
        foreach (int i in _rightList)
        {
            if (!occurancesOf.TryGetValue(i, out var count))
            {
                occurancesOf.Add(i, 0);
            }
            occurancesOf[i] = count + 1;

        }

        foreach (int i in _leftList)
        {
            if (occurancesOf.TryGetValue(i, out var count))
            {
                sum += i * count;
            }
        }

        Console.WriteLine(sum);
    }
}