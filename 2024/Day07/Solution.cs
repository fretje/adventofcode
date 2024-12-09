using System.Collections.Concurrent;

namespace AdventOfCode.Y2024.Day07;

[ProblemName("Bridge Repair")]
internal class Solution : Solver
{
    public object PartOne(string[] lines) =>
        SumValidEquations(lines, ['+', '*']);

    public object PartTwo(string[] lines) =>
        SumValidEquations(lines, ['+', '*', '|']);

    private static long SumValidEquations(string[] lines, char[] possibleOperators)
    {
        ConcurrentBag<long> results = [];
        Parallel.ForEach(Parse(lines), new() { MaxDegreeOfParallelism = -1 }, line =>
        {
            if (HasValidEquation(line.TestValue, line.Numbers, possibleOperators))
            {
                results.Add(line.TestValue);
            }
        });
        return results.Sum();
    }

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
