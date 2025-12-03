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
                var start = long.Parse(parts[0]);
                var end = long.Parse(parts[1]);
                return Range(start, end)
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
        if (str.Length % 2 != 0)
        {
            return false;
        }

        var left = str[..(str.Length / 2)];
        var right = str[(str.Length / 2)..];

        return left == right;
    }

    private static bool IsInvalidPartTwo(long number)
    {
        var str = number.ToString();
        for (int segmentCount = 2; segmentCount <= str.Length; segmentCount++)
        {
            if (str.Length % segmentCount != 0)
            {
                continue;
            }

            var segmentLength = str.Length / segmentCount;
            var segments = Enumerable.Range(0, segmentCount)
                .Select(segmentIndex => str[(segmentIndex * segmentLength)..((segmentIndex + 1) * segmentLength)]);
            var first = segments.First();
            if (segments.Skip(1).All(s => s == first))
            {
                return true;
            }
        }

        return false;
    }
}
