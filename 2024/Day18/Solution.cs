using System.Diagnostics;

namespace AdventOfCode.Y2024.Day18;

[ProblemName("RAM Run")]
class Solution : Solver 
{            
    private const int Width = 71;
    private const int Height = 71;
    private const int FirstBytes = 1024;

    public object PartOne(string[] lines) 
    {
        var bytes = ParseInput(lines);
        var grid = GetGrid(Width, Height, bytes[..FirstBytes]);
        return GetMinimumSteps(grid) ?? 0;
    }

    public object PartTwo(string[] lines)
    {
        var bytes = ParseInput(lines);
        var grid = GetGrid(Width, Height, bytes[..FirstBytes]);
        foreach (var b in bytes[FirstBytes..])
        {
            grid.SetValueAt(b, '#');
            if (GetMinimumSteps(grid) is null)
            {
                return $"{b.Col},{b.Row}";
            }
        }
        throw new UnreachableException();
    }

    private static Pos[] ParseInput(string[] lines) =>
        [.. lines.Select(l => l.Split(",")).Select(l => new Pos(int.Parse(l[0]), int.Parse(l[1])))];

    private static char[][] GetGrid(int width, int height, Pos[] firstBytes) => 
        Enumerable.Range(0, height)
            .Select(row => Enumerable.Range(0, width)
                .Select(col => firstBytes.Contains(new Pos(col, row)) ? '#' : '.')
                .ToArray())
            .ToArray();

    private static int? GetMinimumSteps(char[][] grid)
    {
        List<int> solutions = [];
        Queue<(Pos Pos, int Steps)> queue = [];
        Dictionary<Pos, int> seen = [];
        var start = new Pos(0, 0);
        var end = new Pos(grid[0].Length - 1, grid.Length - 1);
        queue.Enqueue((start, 0));
        seen[start] = 0;
        while (queue.Count != 0)
        {
            var (pos, price) = queue.Dequeue();
            if (!grid.Contains(pos) || grid.ValueAt(pos) == '#')
            {
                continue;
            }
            if (pos == end)
            {
                solutions.Add(price);
                continue;
            }
            foreach (var dir in Directions.All)
            {
                var newPos = pos + dir;
                var newPrice = price + 1;
                if (!seen.TryGetValue(newPos, out var seenPrice) || seenPrice > newPrice)
                {
                    seen[newPos] = price;
                    queue.Enqueue((newPos, newPrice));
                }
            }
        }
        return solutions.Count == 0 ? null : solutions.Min();
    }
}
