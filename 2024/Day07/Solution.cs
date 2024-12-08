namespace AdventOfCode.Y2024.Day07;

[ProblemName("Bridge Repair")]
internal class Solution : Solver
{
    public object PartOne(string[] lines) =>
        CountValidEquations(lines, ['+', '*']);

    public object PartTwo(string[] lines) =>
        CountValidEquations(lines, ['+', '*', '|']);

    private static long CountValidEquations(string[] lines, char[] possibleOperators) => 
        Parse(lines)
            .Sum(line => HasValidEquation(line.TestValue, line.Numbers, possibleOperators) 
                    ? line.TestValue : 0);

    private static IEnumerable<(long TestValue, long[] Numbers)> Parse(string[] lines) =>
        lines.Select(line =>
        {
            var parts = line.Split(": ");
            return (long.Parse(parts[0]), parts[1].Split(' ').Select(long.Parse).ToArray());
        });

    private static bool HasValidEquation(long testValue, long[] numbers, char[] possibleOperators) =>
        possibleOperators.GetPermutations(numbers.Length - 1)
            .Any(operators => Calculate(numbers, operators) == testValue);

    private static long Calculate(long[] numbers, ICollection<char> operators)
    {
        var result = numbers[0];
        for (int i = 0; i < operators.Count; i++)
        {
            var num = numbers[i + 1];
            result = operators.ElementAt(i) switch
            {
                '+' => result + num,
                '*' => result * num,
                '|' => long.Parse($"{result}{num}"),
                _ => throw new InvalidOperationException()
            };
        }
        return result;
    }
}
