namespace AdventOfCode.Y2024.Day25;

[ProblemName("Code Chronicle")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        List<int[]> keys = [];
        List<int[]> locks = [];
        foreach (var lockOrKey in lines.Chunk(8))
        {
            int[] heights = [.. Enumerable.Range(0, 5).Select(col => lockOrKey.Count(l => l.Length > col && l[col] == '#'))];
            if (lockOrKey[0] == "#####")
            {
                locks.Add(heights);
            }
            else
            {
                keys.Add(heights);
            }
        }

        var count = 0;
        foreach (var key in keys)
        {
            foreach (var @lock in locks) 
            {
                if (KeyFitsLock(key, @lock))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static bool KeyFitsLock(int[] key, int[] @lock) =>
        key.Zip(@lock).All(pair => pair.First + pair.Second <= 7);

    public object PartTwo(string[] lines) => null!;
}
