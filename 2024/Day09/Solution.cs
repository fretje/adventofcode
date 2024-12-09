namespace AdventOfCode.Y2024.Day09;

[ProblemName("Disk Fragmenter")]
class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var blocks = GetBlocks(lines[0]);

        int numberOfFileBlocks = blocks.Count(b => b != -1);
        for (int i = blocks.Count - 1; i >= numberOfFileBlocks; i--)
        {
            if (blocks[i] != -1)
            {
                var gapIndex = blocks.IndexOf(-1);
                blocks[gapIndex] = blocks[i];
                blocks[i] = -1;
            }
        }

        return CalculateChecksum(blocks);
    }

    public object PartTwo(string[] lines)
    {
        var blocks = GetBlocks(lines[0]);

        Dictionary<int, List<int>> files = [];
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] != -1)
            {
                if (!files.TryGetValue(blocks[i], out var fileBlocks))
                {
                    fileBlocks = [];
                    files[blocks[i]] = fileBlocks;
                }

                fileBlocks.Add(i);
            }
        }

        foreach (var fileIndex in files.Keys.OrderDescending())
        {
            var fileBlocks = files[fileIndex];
            if (FirstGapWithSize(fileBlocks.Count, fileBlocks[0]) is { } gapIndex && gapIndex > -1)
            {
                for (int i = 0; i < fileBlocks.Count; i++)
                {
                    blocks[gapIndex + i] = fileIndex;
                    blocks[fileBlocks[i]] = -1;
                }
            }
        }

        int FirstGapWithSize(int size, int before)
        {
            for (int i = 0; i < before && i < blocks.Count; i++)
            {
                if (blocks[i] == -1 
                    && i + size <= blocks.Count 
                    && blocks.GetRange(i, size).All(b => b == -1))
                {
                    return i;
                }
            }
            return -1;
        }

        return CalculateChecksum(blocks);
    }

    private static List<int> GetBlocks(string line)
    {
        var sizes = line.Select(c => int.Parse([c])).ToArray();
        var blocks = new List<int>(); // contains file index ==> -1 means gap
        var fileIndex = 0;
        for (int i = 0; i < sizes.Length; i++)
        {
            if (i % 2 == 0) // even => file
            {
                for (int j = 0; j < sizes[i]; j++)
                {
                    blocks.Add(fileIndex);
                }
                fileIndex++;
            }
            else // odd => gap
            {
                for (int j = 0; j < sizes[i]; j++)
                {
                    blocks.Add(-1);
                }
            }
        }
        return blocks;
    }

    private static long CalculateChecksum(List<int> blocks) => 
        blocks.Select((b, i) => (b, i)).Where(x => x.b != -1).Select(x => (long)x.b * x.i).Sum();
}
