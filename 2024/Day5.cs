using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;

class Day5
{
    private readonly List<string> input = ReadInput.CreateListForDay(5);
    private readonly List<List<int>> updates = new(100);
    private Dictionary<int, List<int>> rules = new();
    private readonly List<List<int>> outOfOrderUpdates = new();

    public void PopulateDictionary(List<string> input)
    {
        bool sortedRules = false;
        foreach (string s in input)
        {
            if (!sortedRules) //do rules
            {
                if (s == "")
                {
                    sortedRules = true;
                }
                else
                {
                    int keyBuffer = int.Parse(s.Substring(0, 2));
                    int ValueBuffer = int.Parse(s.Substring(3, 2));

                    if (!rules.TryGetValue(keyBuffer, out var ints)) //no pair
                    {
                        rules[keyBuffer] = new List<int> { ValueBuffer };

                    }
                    else
                    {
                        rules[keyBuffer].Add(ValueBuffer);
                    }

                }
            }
            else //do updates
            {
                string[] updateString = s.Split(',');
                List<int> updateInts = updateString.Select(int.Parse).ToList();
                updates.Add(updateInts);
            }

        }
    }

    public void Part1()
    {
        int sum = 0;
        PopulateDictionary(input);

        foreach (List<int> ints in updates)
        {
            bool isOrdered = true;

            for (int i = 1; i < ints.Count; i++) //0 index doesn't have to check anything
            {
                for (int j = 0; j < i; j++) //only look at keys to the left
                {
                    if (rules.TryGetValue(ints[i], out List<int> values))
                    {
                        values = rules[ints[i]];
                        foreach (int k in values)
                        {
                            if (ints[j] == k)
                            {
                                isOrdered = false;
                                int pageBuffer = ints[i];
                                ints[i] = ints[j];
                                ints[j] = pageBuffer;
                                outOfOrderUpdates.Add(ints);
                            }
                        }
                    }
                }
            }
            if (isOrdered)
            {

                sum += ints[ints.Count / 2];
            }
        }
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        int sum = 0;
        foreach (List<int> ints in outOfOrderUpdates.Distinct()) //duplicates of last ints item are in this list
        {
            sum += ints[ints.Count / 2];
        }
        Console.WriteLine(sum);
    }
}