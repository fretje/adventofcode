﻿namespace AdventOfCode.Y2024.Day01;

[ProblemName("Historian Hysteria")]
internal class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var (leftColumn, rightColumn) = Parse(lines);
        return leftColumn.Order().Zip(rightColumn.Order())
            .Sum(x => Math.Abs(x.First - x.Second));
    }

    public object PartTwo(string[] lines)
    {
        var (leftColumn, rightColumn) = Parse(lines);
        return leftColumn.Sum(left => left * rightColumn.Count(right => right == left));
    }

    private static (List<int> LeftColumn, List<int> RightColumn) Parse(string[] lines) =>
        lines
            .Select(line => line.Split("   "))
            .Select(parts => (Left: int.Parse(parts[0]), Right: int.Parse(parts[1])))
            .Aggregate((LeftColumn: new List<int>(), RightColumn: new List<int>()), (acc, line) =>
            {
                acc.LeftColumn.Add(line.Left);
                acc.RightColumn.Add(line.Right);
                return acc;
            });
}