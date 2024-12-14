class Day4
{
    private readonly List<string> _input = ReadInput.CreateListForDay(4);

    private int searchHorizontal(List<string> wordSearch)
    {
        int matches = 0;
        foreach (string s in wordSearch)
        {
            if (s[0] != '.')
            {
                for (int i = 0; i < wordSearch[0].Length - 3; i++)
                {
                    if (s.Substring(i, 4) == "XMAS" || s.Substring(i, 4) == "SAMX")
                    {
                        matches += 1;
                    }
                }
            }
        }
        return matches;
    }

    private int searchVertical(List<string> wordSearch)
    {
        int matches = 0;

        for (int i = 0; i < wordSearch.Count - 3; i++)
        {
            for (int j = 0; j < wordSearch[0].Length - 3; j++)
            {
                string verticalString = $"{wordSearch[i][j]}{wordSearch[i + 1][j]}{wordSearch[i + 2][j]}{wordSearch[i + 3][j]}";
                if (verticalString == "XMAS" || verticalString == "SAMX")
                {
                    matches += 1;
                }
            }
        }
        return matches;
    }

    private int searchDiagonal(List<string> wordSearch)
    {
        int matches = 0;
        for (int i = 0; i < wordSearch.Count - 3; i++) //top left to bottom right
        {
            for (int j = 0; j < wordSearch.Count - 3; j++)
            {
                string diagonalString = $"{wordSearch[i][j]}{wordSearch[i + 1][j + 1]}{wordSearch[i + 2][j + 2]}{wordSearch[i + 3][j + 3]}";
                if (diagonalString == "XMAS" || diagonalString == "SAMX")
                {
                    matches += 1;
                }
            }
        }

        for (int i = wordSearch.Count - 3; i >= 3; i--) //bottom left to top right
        {
            for (int j = 0; j < wordSearch.Count - 3; j++)
            {
                string diagonalString = $"{wordSearch[i][j]}{wordSearch[i - 1][j + 1]}{wordSearch[i - 2][j + 2]}{wordSearch[i - 3][j + 3]}";
                if (diagonalString == "XMAS" || diagonalString == "SAMX")
                {
                    matches += 1;
                }
            }
        }
        return matches;
    }

    private int searchX(List<string> wordSearch)
    {
        int matches = 0;
        for (int i = 1; i < wordSearch.Count - 3; i++)
        {
            for (int j = 1; j < wordSearch.Count - 3; j++)
            {
                if (wordSearch[i][j] == 'A')
                {
                    string xString = $"{wordSearch[i - 1][j - 1]}{wordSearch[i - 1][j + 1]}{wordSearch[i + 1][j - 1]}{wordSearch[i + 1][j + 1]}";
                    if (xString == "MSMS" || xString == "SMSM" || xString == "MMSS" || xString == "SSMM")
                    {
                        matches += 1;
                    }
                }
            }
        }
        return matches;
    }

    private List<string> AddPadding(List<string> wordSearch) //prevent going out of range
    {
        for (int i = 0; i < wordSearch.Count; i++)
        {
            wordSearch[i] += "...";
        }
        for (int i = 0; i < 3; i++)
        {
            wordSearch.Add(string.Concat(Enumerable.Repeat('.', wordSearch[0].Length)));
        }

        return wordSearch;
    }

    public void Part1()
    {
        int sum = 0;
        var newSearch = AddPadding(_input);
        sum += searchHorizontal(newSearch) + searchVertical(newSearch) + searchDiagonal(newSearch);
        Console.WriteLine(sum);
    }

    public void Part2()
    {
        int sum = 0;
        var newSearch = AddPadding(_input);
        sum += searchX(newSearch);
        Console.WriteLine(sum);
    }

}

/* nonfunctional for real input
    Regex xmasPattern = new Regex(@"(?=XMAS|SAMX)");

        private int HorizontalMatch(List<string> wordSearch)
        {
            int matches = 0;
            foreach (string line in wordSearch)
            {
                matches += xmasPattern.Matches(line).Count;
            }
            return matches;
        }

        private int VerticalMatch(List<string> wordSearch)
        {
            List<string> newSearch = new(wordSearch.Count);
            for (int i = 0; i < newSearch.Capacity; i++)
            {
                newSearch.Add("");
            }

            foreach (string line in wordSearch) //rotate wordSearch 90 degrees
            {
                for (int i = 0; i < line.Length; i++)
                {
                    newSearch[i] += line[i];
                }
            }
            return HorizontalMatch(newSearch);
        }

        private int DiagonalMatch(List<string> wordSearch)
        {
            List<string> newSearch = new(wordSearch.Count * 2);
            for (int i = 0; i < newSearch.Capacity; i++)
            {
                newSearch.Add("");
            }

            //top diagonal half cut of wordSearch
            for (int i = 0; i < wordSearch[0].Length; i++) //loop 10 times (for each starting char)
            {
                for (int j = i; j < wordSearch[0].Length; j++) //loop 10, then 9, then 8, then 7 times, etc.) cuts 2 lines per ?
                {
                    newSearch[i] += wordSearch[j - i][j];
                }
            }
            //loop 9 times starting at i=1, once per line, 
            for (int i = 1; i < wordSearch[0].Length; i++)
            {
                for (int j = i; j < wordSearch[0].Length; j++)
                {
                    newSearch[i + wordSearch[0].Length] += wordSearch[j][j - i];
                }
            }

            List<string> bufferSearch = new();
            foreach (string s in newSearch)
            {
                bufferSearch.Add(new string(s.Reverse().ToArray()));
            }



            newSearch.AddRange(bufferSearch);

            return HorizontalMatch(newSearch);
        }

        public void Part1()
        {
            int sum = 0;
            sum += HorizontalMatch(_input) + VerticalMatch(_input) + DiagonalMatch(_input);
            Console.WriteLine(sum);
        } */