namespace AdventOfCode.Y2024.Day03;

[ProblemName("Mull It Over")]
internal partial class Solution : Solver
{
    public object PartOne(string[] lines) => lines.Sum(GetMultiplications);

    public object PartTwo(string[] lines) => GetMultiplications2(string.Join("", lines));

    private static int GetMultiplications(string line) =>
        MulRegex().Matches(line).AsEnumerable()
            .Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));

    [GeneratedRegex(@"mul\((\d*),(\d*)\)")]
    private static partial Regex MulRegex();

    private static int GetMultiplications2(string line)
    {
        var multiplications = 0;
        var rest = line;
        while (rest.Length > 0)
        {
            var dontIndex = rest.IndexOf("don't()");
            if (dontIndex == -1)
            {
                return multiplications + GetMultiplications(rest);
            }
            var doPart = rest[0..dontIndex];
            multiplications += GetMultiplications(doPart);
            rest = rest[(dontIndex + 7)..];
            var doIndex = rest.IndexOf("do()");
            if (doIndex == -1)
            {
                return multiplications;
            }
            rest = rest[(doIndex + 4)..];
        }
        return multiplications;
    }
}
