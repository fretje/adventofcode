namespace AdventOfCode.Y2024.Day04;

[ProblemName("Ceres Search")]
internal partial class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        return CountMatches(lines
            .Concat(GetVerticals(lines))
            .Concat(GetDiagonals(lines))
            .Concat(GetDiagonals(GetReverseLines(lines))));

        int CountMatches(IEnumerable<string> lines) =>
            lines.Sum(line => XmasRegex().Matches(line).Count + XmasRegex().Matches(line.ReverseString()).Count);

        static IEnumerable<string> GetVerticals(string[] lines) =>
            Enumerable.Range(0, lines[0].Length)
                .Select(col => new string([.. lines.Select(line => line[col])]));

        static IEnumerable<string> GetDiagonals(string[] lines)
        {
            var rowCount = lines.Length;
            var colCount = lines[0].Length;
            for (var line = 1; line <= (rowCount + colCount - 1); line++)
            {
                var startCol = Math.Max(0, line - rowCount);
                var count = Math.Min(line, Math.Min(colCount - startCol, rowCount));
                yield return new([.. Enumerable.Range(0, count)
                    .Select(i => lines[Math.Min(rowCount, line) - i - 1][startCol + i])]);
            }
        }

        static string[] GetReverseLines(string[] lines) => [.. lines.Select(l => l.ReverseString())];
    }

    [GeneratedRegex("XMAS")]
    private static partial Regex XmasRegex();

    public object PartTwo(string[] lines)
    {
        var grid = lines.ToGrid();
        return grid.AllCells()
            .Where(c => c.Value == 'A')
            .Select(c => c.Pos)
            .Count(pos =>
                ((grid.ValueAt(pos, new Pos(-1, -1)) == 'M' && grid.ValueAt(pos, new Pos(1, 1)) == 'S')
                    || (grid.ValueAt(pos, new Pos(-1, -1)) == 'S' && grid.ValueAt(pos, new Pos(1, 1)) == 'M'))
                && ((grid.ValueAt(pos, new Pos(-1, 1)) == 'M' && grid.ValueAt(pos, new Pos(1, -1)) == 'S')
                    || (grid.ValueAt(pos, new Pos(-1, 1)) == 'S' && grid.ValueAt(pos, new Pos(1, -1)) == 'M')));
    }
}
