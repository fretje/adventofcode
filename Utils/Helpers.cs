using System.Diagnostics;

namespace AdventOfCode;

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

    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptyProduct = [[]];
        return sequences.Aggregate(
            emptyProduct,
            (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat([item]));
    }

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

    public static IEnumerable<T[]> GetDiagonals<T>(this T[][] lines)
    {
        var rowCount = lines.Length;
        var colCount = lines[0].Length;
        for (var line = 1; line <= (rowCount + colCount - 1); line++)
        {
            var startCol = Math.Max(0, line - rowCount);
            var count = Math.Min(line, Math.Min(colCount - startCol, rowCount));
            yield return [.. Enumerable.Range(0, count)
                .Select(i => lines[Math.Min(rowCount, line) - i - 1][startCol + i])];
        }
    }

    public static int NumberOfDigits(this long value)
    {
        if (value >= 0)
        {
            if (value < 10L) return 1;
            if (value < 100L) return 2;
            if (value < 1000L) return 3;
            if (value < 10000L) return 4;
            if (value < 100000L) return 5;
            if (value < 1000000L) return 6;
            if (value < 10000000L) return 7;
            if (value < 100000000L) return 8;
            if (value < 1000000000L) return 9;
            if (value < 10000000000L) return 10;
            if (value < 100000000000L) return 11;
            if (value < 1000000000000L) return 12;
            if (value < 10000000000000L) return 13;
            if (value < 100000000000000L) return 14;
            if (value < 1000000000000000L) return 15;
            if (value < 10000000000000000L) return 16;
            if (value < 100000000000000000L) return 17;
            if (value < 1000000000000000000L) return 18;
            return 19;
        }
        else
        {
            if (value > -10L) return 2;
            if (value > -100L) return 3;
            if (value > -1000L) return 4;
            if (value > -10000L) return 5;
            if (value > -100000L) return 6;
            if (value > -1000000L) return 7;
            if (value > -10000000L) return 8;
            if (value > -100000000L) return 9;
            if (value > -1000000000L) return 10;
            if (value > -10000000000L) return 11;
            if (value > -100000000000L) return 12;
            if (value > -1000000000000L) return 13;
            if (value > -10000000000000L) return 14;
            if (value > -100000000000000L) return 15;
            if (value > -1000000000000000L) return 16;
            if (value > -10000000000000000L) return 17;
            if (value > -100000000000000000L) return 18;
            if (value > -1000000000000000000L) return 19;
            return 20;
        }
    }
}
