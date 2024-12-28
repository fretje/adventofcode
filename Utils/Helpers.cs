using System.Diagnostics;

namespace AdventOfCode;

public static class Helpers
{
    public static string ReverseString(this string input) => new([.. input.Reverse()]);

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
