namespace AdventOfCode.Y2024.Day10;

[ProblemName("Hoof It")]
class Solution : Solver 
{            
    private static readonly Pos[] _directions = [new Pos(1, 0), new Pos(0, 1), new Pos(-1, 0), new Pos(0, -1)];

    public object PartOne(string[] lines) => CalculateScore(lines);

    public object PartTwo(string[] lines) => CalculateScore(lines, part2: true);

    private static int CalculateScore(string[] lines, bool part2 = false)
    {
        var grid = ParseGrid(lines);
        return grid.AllCells().Where(cell => cell.Value is 0).Sum(cell => CalculateScore(cell.Pos, grid, part2: part2));
    }

    private static int[][] ParseGrid(string[] lines) => 
        lines.Select(line => line.Select(ch => ch == '.' ? -1 : int.Parse([ch])).ToArray()).ToArray();

    private static int CalculateScore(Pos pos, int[][] grid, HashSet<Pos>? visited = null, bool part2 = false)
    {
        if (!part2)
        {
            visited ??= [];
            if (!grid.Contains(pos) || visited.Contains(pos))
            {
                return 0;
            }
            visited.Add(pos);
        }

        var value = grid.ValueAt(pos);
        if (value == 9)
        {
            return 1;
        }

        return _directions
            .Where(dir => grid.ValueAt(pos, dir) == value + 1)
            .Sum(dir => CalculateScore(new Pos(pos.Col + dir.Col, pos.Row + dir.Row), grid, visited, part2));
    }
}
