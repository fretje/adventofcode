namespace AdventOfCode;

internal static class GridExtensions
{
    public static char[][] ToGrid(this string[] lines) => [.. lines.Select(line => line.ToCharArray())];

    public static IEnumerable<(T Value, Pos Pos)> AllCells<T>(this T[][] grid)
    {
        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                yield return (grid[row][col], new(col, row));
            }
        }
    }

    public static T? ValueAt<T>(this T[][] grid, Pos pos) =>
        grid.Contains(pos) ? grid[pos.Row][pos.Col] : default;

    public static void SetValueAt<T>(this T[][] grid, Pos pos, T value) =>
        grid[pos.Row][pos.Col] = value;

    public static bool Contains<T>(this T[][] grid, Pos pos) =>
        pos.Row >= 0 && pos.Row < grid.Length && pos.Col >= 0 && pos.Col < grid[0].Length;

    public static T[][] DeepClone<T>(this T[][] grid) =>
        grid.Select(r => r.ToArray()).ToArray();

    public static int? GetMinimumSteps(this char[][] grid, Pos start, Pos end)
    {
        Queue<(Pos, int)> queue = [];
        queue.Enqueue((start, 0));
        HashSet<Pos> seen = [start];
        while (queue.Count != 0)
        {
            var (pos, steps) = queue.Dequeue();
            foreach (var dir in Directions.Othogonal)
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
