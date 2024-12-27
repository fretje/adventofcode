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
        Dictionary<int, List<int>> files = []; // maps file index to block indexes
        List<(int BlockIndex, int Size)> gaps = [];
        for (int blockIndex = 0; blockIndex < blocks.Count; blockIndex++)
        {
            if (blocks[blockIndex] != -1)
            {
                if (!files.TryGetValue(blocks[blockIndex], out var fileBlocks))
                {
                    fileBlocks = [];
                    files[blocks[blockIndex]] = fileBlocks;
                }
                fileBlocks.Add(blockIndex);
            }
            else
            {
                var gapBlockIndex = blockIndex;
                while (blockIndex < blocks.Count - 1 && blocks[blockIndex + 1] == -1)
                {
                    blockIndex++;
                }
                gaps.Add((gapBlockIndex, blockIndex - gapBlockIndex + 1));
            }
        }
        foreach (var fileIndex in files.Keys.OrderDescending())
        {
            var fileBlocks = files[fileIndex];
            if (gaps.FirstOrDefault(gap => gap.Size >= fileBlocks.Count && gap.BlockIndex < fileBlocks[0]) is var gap && gap != default)
            {
                for (int i = 0; i < fileBlocks.Count; i++)
                {
                    blocks[gap.BlockIndex + i] = fileIndex;
                    blocks[fileBlocks[i]] = -1;
                }
                var gapIndex = gaps.IndexOf(gap);
                gaps.Remove(gap);
                if (gap.Size > fileBlocks.Count)
                {
                    gaps.Insert(gapIndex, (gap.BlockIndex + fileBlocks.Count, gap.Size - fileBlocks.Count));
                }
            }
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
