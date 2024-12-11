namespace AdventOfCode.Y2024.Day11;

[ProblemName("Plutonian Pebbles")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        List<long> input = [.. lines[0].Split(' ').Select(long.Parse)];
        for (int i = 0; i < 25; i++)
        {
            input = GenerateNext(input);
        }
        return input.Count;
    }

    private static List<long> GenerateNext(List<long> input)
    {
        List<long> next = [];
        foreach (long value in input)
        {
            next.AddRange(value switch
            {
                0 => [1],
                _ when value.NumberOfDigits() % 2 == 0 && value.ToString() is { } str =>
                    [long.Parse(str[0..(str.Length / 2)]), long.Parse(str[(str.Length / 2)..])],
                _ => [value * 2024]
            });
        }
        return next;
    }

    public object PartTwo(string[] lines) =>
        lines[0].Split(' ').Select(long.Parse).Sum(i => StoneCountAfter(i, 75));

    private readonly Dictionary<(long, int), long> _cachedResults = [];

    public long StoneCountAfter(long value, int generations)
    {
        if (_cachedResults.TryGetValue((value, generations), out var result))
        {
            return result;
        }

        result = (value, generations) switch
        {
            (_, 0) => 1,
            (0, _) => StoneCountAfter(1, generations - 1),
            _ when value.NumberOfDigits() % 2 == 0 && value.ToString() is { } str =>
                StoneCountAfter(long.Parse(str[0..(str.Length / 2)]), generations - 1)
                + StoneCountAfter(long.Parse(str[(str.Length / 2)..]), generations - 1),
            _ => StoneCountAfter(value * 2024, generations - 1)
        };

        _cachedResults[(value, generations)] = result;

        return result;
    }
}
