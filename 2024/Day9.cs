class Day9
{
    private readonly string input = ReadInput.CreateListForDay(9)[0];
    record struct File(int ID, int Count); //index on drive is ID * 2 for real files
    private List<File> files = new();
    private int maxID = 0;

    private void PopulateFileList(string input)
    {
        files.Clear();
        for (int i = 0; i < input.Length; i++)
        {
            if (i % 2 == 0) //file
            {
                files.Add(new File(i / 2, input[i] - '0'));
                maxID = i / 2;
            }
            else //blank space
            {
                files.Add(new File(-1, input[i] - '0'));
            }
        }
    }

    private long GetChecksum()
    {
        int diskIndex = 0;
        long sum = 0;
        foreach (var file in files)
        {
            for (int i = file.Count; i > 0; i--)
            {
                if (file.ID != -1)
                {
                    sum += file.ID * diskIndex;
                }
                diskIndex++;
            }
        }
        return sum;
    }

    private void DefragmentWholeFiles()
    {
        for (int i = maxID; i > 0; i--)
        {
            var maxIDFile = files.Find(file => file.ID == i);
            var maxIDIndex = files.FindIndex(file => file.ID == i);
            for (int j = 0; j < maxIDIndex; j++)
            {
                if (files[j].ID == -1)
                {
                    if (files[j].Count == maxIDFile.Count)
                    {
                        files[j] = new File(maxIDFile.ID, maxIDFile.Count);
                        files[maxIDIndex] = new File(-1, maxIDFile.Count);
                    }
                    else if (files[j].Count > maxIDFile.Count) //more free space than files
                    {
                        files[j] = new File(files[j].ID, files[j].Count - maxIDFile.Count);
                        files.Insert(j, new File(maxIDFile.ID, maxIDFile.Count));
                        maxIDIndex++; //since we just inserted a File
                        files[maxIDIndex] = new File(-1, maxIDFile.Count);
                    }
                    break;
                }
            }
        }
    }

    private void DefragmentFiles()
    {
        int endIndex = files.Count - 1;
        for (int i = 0; i < files.Count; i++)
        {
            //we don't have to search past the last number we sorted, since they'll all be blank
            if (files[i].ID == -1) //blank space found
            {
                for (int j = endIndex; j > i; j--)
                {
                    if (files[j].ID != -1)
                    {
                        if (files[i].Count > files[j].Count) //more blank space than file space
                        {
                            files[i] = new File(files[i].ID, files[i].Count - files[j].Count);
                            files.Insert(i, new File(files[j].ID, files[j].Count));
                            j++; //since we just added a File
                            files[j] = new File(-1, files[j].Count);
                        }
                        else if (files[i].Count == files[j].Count)
                        {
                            files[i] = new File(files[j].ID, files[j].Count);
                            files[j] = new File(-1, files[j].Count);
                        }
                        else if (files[j].Count > files[i].Count) //more files than blank space
                        {
                            files[i] = new File(files[j].ID, files[i].Count);
                            files[j] = new File(files[j].ID, files[j].Count - files[i].Count);
                            files.Insert(j + 1, new File(-1, files[i].Count));
                        }
                        endIndex = j;
                        break;
                    }
                }
            }
        }
    }

    public void Part1()
    {
        PopulateFileList(input);
        DefragmentFiles();
        Console.WriteLine($"Part 1: {GetChecksum()}");
    }
    public void Part2()
    {
        PopulateFileList(input);
        DefragmentWholeFiles();
        Console.WriteLine($"Part 2: {GetChecksum()}");
    }

    private void PrintDisk() //debug
    {
        foreach (var file in files)
        {
            string disk = "";
            for (int i = 0; i < file.Count; i++)
            {
                if (file.ID == -1)
                {
                    disk += '.';
                }
                else
                {
                    disk += file.ID;
                }
            }
            Console.Write($"{disk}");
        }
        Console.WriteLine();
    }
}

