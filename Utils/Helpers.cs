using System.Diagnostics;

namespace AdventOfCode;

public record struct Pos(int Col, int Row);

public static class Helpers
{
    public static string ReverseString(this string input) => new([.. input.Reverse()]);

    public static char[][] ToGrid(this string[] lines) => [.. lines.Select(line => line.ToCharArray())];

    public static IEnumerable<(T Value, Pos Pos)> AllCells<T>(this T[][] grid)
    {
        for (var row = 0; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[row].Length; col++)
            {
                yield return (grid[row][col], new Pos(col, row));
            }
        }
    }

    public static T? ValueAt<T>(this T[][] grid, Pos pos, Pos? delta = null)
    {
        delta ??= new Pos(0, 0);
        var row = pos.Row + delta.Value.Row;
        var col = pos.Col + delta.Value.Col;
        return row < 0 || row >= grid.Length || col < 0 || col >= grid[0].Length
            ? default
            : grid[row][col];
    }

    public static void SetValueAt<T>(this T[][] grid, Pos pos, T value) =>
        grid[pos.Row][pos.Col] = value;

    public static bool Contains<T>(this T[][] grid, Pos pos) =>
        pos.Row >= 0 && pos.Row < grid.Length && pos.Col >= 0 && pos.Col < grid[0].Length;

    public static T[][] DeepClone<T>(this T[][] grid) =>
        grid.Select(r => r.ToArray()).ToArray();

    public static IEnumerable<(T, T)> GetCombinations<T>(this ICollection<T> input)
    {
        for (var i = 0; i < input.Count; i++)
        {
            for (var j = i + 1; j < input.Count; j++)
            {
                yield return (input.ElementAt(i), input.ElementAt(j));
            }
        }
    }

    public static ICollection<ICollection<T>> GetPermutations<T>(this ICollection<T> input, int outputLength)
    {
        Debug.Assert(outputLength > 0);
        List<T[]> permutations = [.. input.Select(item => (T[])[item])];
        for (var i = 1; i < outputLength; i++)
        {
            List<T[]> newPermutations = [];
            foreach (var perm in permutations)
            {
                foreach (var item in input)
                {
                    newPermutations.Add([.. perm.Concat([item])]);
                }
            }
            permutations = newPermutations;
        }
        return [.. permutations];
    }
}
