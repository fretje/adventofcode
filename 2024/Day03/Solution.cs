using System.Diagnostics;

namespace AdventOfCode.Y2024.Day03;

[ProblemName("Mull It Over")]
internal partial class Solution : Solver
{
    public object PartOne(string[] lines) => lines.Sum(GetMultiplications);

    public object PartTwo(string[] lines)
    {
        var sum = 0;
        var rest = string.Join("", lines);
        while (rest.Length > 0)
        {
            var dontIndex = rest.IndexOf("don't()");
            if (dontIndex == -1)
            {
                return sum + GetMultiplications(rest);
            }
            sum += GetMultiplications(rest[0..dontIndex]);
            rest = rest[(dontIndex + 7)..];
            var doIndex = rest.IndexOf("do()");
            if (doIndex == -1)
            {
                return sum;
            }
            rest = rest[(doIndex + 4)..];
        }
        throw new UnreachableException();
    }

    private static int GetMultiplications(string line) =>
        MulRegex().Matches(line).AsEnumerable()
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));

    [GeneratedRegex(@"mul\((\d*),(\d*)\)")]
    private static partial Regex MulRegex();
}
