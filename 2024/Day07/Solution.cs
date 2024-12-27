namespace AdventOfCode.Y2024.Day07;

[ProblemName("Bridge Repair")]
internal class Solution : Solver
{
    public object PartOne(string[] lines) => SumValidEquations(lines);

    public object PartTwo(string[] lines) => SumValidEquations(lines, true);

    private static long SumValidEquations(string[] lines, bool part2 = false) =>
        lines
            .Select(line =>
            {
                var parts = line.Split(": ");
                return (Expected: long.Parse(parts[0]), Numbers: parts[1].Split(' ').Select(long.Parse).ToArray());
            })
            .Where(x => IsValidEquation(x.Expected, x.Numbers, part2))
            .Sum(x => x.Expected);

    private static bool IsValidEquation(long expected, long[] numbers, bool part2 = false) =>
        numbers.Length == 1
            ? numbers[0] == expected
            : (expected % numbers[^1] == 0 && IsValidEquation(expected / numbers[^1], numbers[0..^1], part2))
                || (expected > numbers[^1] && IsValidEquation(expected - numbers[^1], numbers[0..^1], part2))
                || (part2 && expected.ToString() is { } expectedStr && numbers[^1].ToString() is { } numberStr
                    && expectedStr.Length > numberStr.Length && expectedStr.EndsWith(numberStr)
                    && IsValidEquation(long.Parse(expectedStr[..^numberStr.Length]), numbers[0..^1], part2));
}
