namespace AdventOfCode.Y2024.Day04;

[ProblemName("Ceres Search")]
internal partial class Solution : Solver
{
    public object PartOne(string[] lines) =>
        CountMatches(lines
            .Concat(GetVerticals(lines))
            .Concat(GetDiagonals(lines))
            .Concat(GetDiagonals(GetReverseLines(lines))));

    private static int CountMatches(IEnumerable<string> lines) =>
        lines.Sum(line => XmasRegex().Matches(line).Count + XmasRegex().Matches(line.ReverseString()).Count);

    private static IEnumerable<string> GetVerticals(string[] lines) =>
        Enumerable.Range(0, lines[0].Length)
            .Select(col => new string([.. lines.Select(line => line[col])]));

    private static IEnumerable<string> GetDiagonals(string[] lines) => 
        lines.ToGrid().GetDiagonals().Select(x => new string(x));

    static string[] GetReverseLines(string[] lines) => [.. lines.Select(l => l.ReverseString())];

    [GeneratedRegex("XMAS")]
    private static partial Regex XmasRegex();

    public object PartTwo(string[] lines)
    {
        var grid = lines.ToGrid();
        return grid.AllCells()
            .Where(c => c.Value == 'A')
            .Select(c => c.Pos)
            .Count(pos =>
                ((grid.ValueAt(pos + Directions.UpLeft) == 'M' && grid.ValueAt(pos + Directions.DownRight) == 'S')
                    || (grid.ValueAt(pos + Directions.UpLeft) == 'S' && grid.ValueAt(pos + Directions.DownRight) == 'M'))
                && ((grid.ValueAt(pos + Directions.DownLeft) == 'M' && grid.ValueAt(pos + Directions.UpRight) == 'S')
                    || (grid.ValueAt(pos + Directions.DownLeft) == 'S' && grid.ValueAt(pos + Directions.UpRight) == 'M')));
    }
}
