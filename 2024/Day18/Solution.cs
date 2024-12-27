﻿namespace AdventOfCode.Y2024.Day18;

[ProblemName("RAM Run")]
class Solution : Solver 
{            
    private const int Width = 71;
    private const int Height = 71;
    private const int FirstBytes = 1024;

    public object PartOne(string[] lines) => 
        GetGrid(lines.ParseInput()[..FirstBytes]).GetMinimumSteps() ?? 0;

    public object PartTwo(string[] lines)
    {
        var bytes = lines.ParseInput();
        var low = FirstBytes;
        var hi = bytes.Length - 1;
        while (low < hi)
        {
            var mid = (low + hi) / 2;
            if (GetGrid(bytes[..(mid + 1)]).GetMinimumSteps() is null)
            {
                hi = mid;
            }
            else
            {
                low = mid + 1;
            }
        }
        return $"{bytes[low].Col},{bytes[low].Row}";
    }

    public static char[][] GetGrid(Pos[] corruptBytes) => 
        Enumerable.Range(0, Height)
            .Select(row => Enumerable.Range(0, Width)
                .Select(col => corruptBytes.Contains(new(col, row)) ? '#' : '.')
                .ToArray())
            .ToArray();
}

static class Extensions
{
    public static Pos[] ParseInput(this string[] lines) =>
        [.. lines.Select(l => l.Split(",")).Select(l => new Pos(int.Parse(l[0]), int.Parse(l[1])))];

    public static int? GetMinimumSteps(this char[][] grid)
    {
        Pos start = new(0, 0);
        Pos end = new(grid[0].Length - 1, grid.Length - 1);
        Queue<(Pos, int)> queue = [];
        queue.Enqueue((start, 0));
        HashSet<Pos> seen = [start];
        while (queue.Count != 0)
        {
            var (pos, steps) = queue.Dequeue();
            foreach (var dir in Directions.All)
            {
                var next = pos + dir;
                if (!grid.Contains(next) || grid.ValueAt(next) == '#' || seen.Contains(next))
                {
                    continue;
                }
                if (next == end)
                {
                    return steps + 1;
                }
                seen.Add(next);
                queue.Enqueue((next, steps + 1));
            }
        }
        return null;
    }
}
