namespace AdventOfCode.Y2024.Day22;

[ProblemName("Monkey Market")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        long[] numbers = [.. lines.Select(long.Parse)];
        Parallel.For(0, numbers.Length, n =>
        {
            for (var i = 0; i < 2000; i++)
            {
                numbers[n] = GetNextNumber(numbers[n]);
            }
        });
        return numbers.Sum();
    }

    public object PartTwo(string[] lines) 
    {
        Dictionary<(int, int, int, int), long> seqOf4DiffsTotal = [];
        foreach (var n in lines.Select(long.Parse))
        {
            var number = n;
            List<int> prices = [(int)number % 10];
            for (var i = 1; i <= 2000; i++)
            {
                number = GetNextNumber(number);
                prices.Add((int)number % 10);
            }
            HashSet<(int, int, int, int)> seen = [];
            int[] differences = [.. prices.Zip(prices.Skip(1), (a, b) => b - a)];
            for (int i = 0; i < differences.Length - 3; i++)
            {
                var seqOf4Diffs = (differences[i], differences[i + 1], differences[i + 2], differences[i + 3]);
                if (seen.Add(seqOf4Diffs))
                {
                    if (!seqOf4DiffsTotal.ContainsKey(seqOf4Diffs))
                    {
                        seqOf4DiffsTotal[seqOf4Diffs] = 0;
                    }
                    seqOf4DiffsTotal[seqOf4Diffs] += prices[i + 4];
                }
            }
        }
        return seqOf4DiffsTotal.Values.Max();
    }

    private static long GetNextNumber(long number)
    {
        number ^= number << 6 & 0xFFFFFF;
        number ^= number >> 5 & 0xFFFFFF;
        number ^= number << 11 & 0xFFFFFF;
        return number;
    }
}
