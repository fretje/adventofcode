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
        Dictionary<(int, int, int, int), long> differencesTotal = [];
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
                var seq = (differences[i], differences[i + 1], differences[i + 2], differences[i + 3]);
                if (seen.Add(seq))
                {
                    if (!differencesTotal.ContainsKey(seq))
                    {
                        differencesTotal[seq] = 0;
                    }
                    differencesTotal[seq] += prices[i + 4];
                }
            }
        }
        return differencesTotal.Values.Max();
    }

    private static long GetNextNumber(long number)
    {
        number ^= number * 64;
        number %= 16777216;
        number ^= number / 32;
        number %= 16777216;
        number ^= number * 2048;
        number %= 16777216;
        return number;
    }
}
