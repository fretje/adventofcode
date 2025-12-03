namespace AdventOfCode.Y2025.Day02;

[ProblemName("Gift Shop")]
class Solution : Solver 
{
    public object PartOne(string[] lines) => Calculate(lines[0]);

    public object PartTwo(string[] lines) => Calculate(lines[0], partTwo: true);

    public static long Calculate(string line, bool partTwo = false) => 
        line
            .Split(',')
            .Sum(range =>
            {
                var parts = range.Split('-');
                return Range(long.Parse(parts[0]), long.Parse(parts[1]))
                    .Where(partTwo ? IsInvalidPartTwo : IsInvalidPartOne)
                    .Sum();
            });

    private static IEnumerable<long> Range(long start, long end)
    {
        for (long i = start; i <= end; i++)
        {
            yield return i;
        }
    }

    private static bool IsInvalidPartOne(long number)
    {
        var str = number.ToString();
        return str.Length % 2 == 0 && str[..(str.Length / 2)] == str[(str.Length / 2)..];
    }

    private static bool IsInvalidPartTwo(long number)
    {
        var str = number.ToString();
        foreach (var segmentCount in Enumerable.Range(2, str.Length - 1))
        {
            if (str.Length % segmentCount == 0 
                && str == string.Concat(Enumerable.Repeat(str[..(str.Length / segmentCount)], segmentCount)))
            {
                return true;
            }
        }

        return false;
    }
}
