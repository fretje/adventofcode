namespace AdventOfCode.Y2024.Day02;

[ProblemName("Red-Nosed Reports")]
internal class Solution : Solver
{
    public object PartOne(string[] lines) => Parse(lines).Count(IsSafe);

    public object PartTwo(string[] lines) => Parse(lines).Count(IsSafeWithDampener);

    private static IEnumerable<int[]> Parse(string[] lines) =>
        lines.Select(line => line.Split(' ').Select(int.Parse).ToArray());

    private static bool IsSafe(int[] line)
    {
        if (line.Length < 2)
        {
            return true;
        }
        var greater = line[1] > line[0];
        if (Math.Abs(line[1] - line[0]) is < 1 or > 3)
        {
            return false;
        }
        for (var i = 2; i < line.Length; i++)
        {
            if ((line[i] > line[i - 1]) != greater
                || Math.Abs(line[i] - line[i - 1]) is < 1 or > 3)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsSafeWithDampener(int[] line)
    {
        if (IsSafe(line))
        {
            return true;
        }
        for (var i = 0; i < line.Length; i++)
        {
            var list = line.ToList();
            list.RemoveAt(i);
            if (IsSafe([.. list]))
            {
                return true;
            }
        }
        return false;
    }
}
