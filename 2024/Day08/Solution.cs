namespace AdventOfCode.Y2024.Day08;

[ProblemName("Resonant Collinearity")]
internal class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var (grid, antennaGroups) = Parse(lines);
        HashSet<Pos> antiNodes = [];
        foreach (var antennaGroup in antennaGroups.Values)
        {
            foreach (var (pos1, pos2) in antennaGroup.GetCombinations())
            {
                if (FindAntiNode(pos1, pos2, grid) is { } antiNode)
                {
                    antiNodes.Add(antiNode);
                }
                if (FindAntiNode(pos2, pos1, grid) is { } antiNode2)
                {
                    antiNodes.Add(antiNode2);
                }
            }
        }
        return antiNodes.Count;
    }

    public object PartTwo(string[] lines)
    {
        var (grid, antennaGroups) = Parse(lines);
        HashSet<Pos> antiNodes = [];
        foreach (var antennaGroup in antennaGroups.Values)
        {
            foreach (var (pos1, pos2) in antennaGroup.GetCombinations())
            {
                antiNodes.Add(pos1);
                antiNodes.Add(pos2);
                antiNodes.UnionWith(FindAntiNodes(pos1, pos2, grid));
                antiNodes.UnionWith(FindAntiNodes(pos2, pos1, grid));
            }
        }
        return antiNodes.Count;
    }

    private static (char[][], Dictionary<char, List<Pos>>) Parse(string[] lines)
    {
        char[][] grid = lines.ToGrid();
        Dictionary<char, List<Pos>> antennaGroups = [];
        foreach (var cell in grid.AllCells().Where(cell => cell.Value != '.'))
        {
            if (!antennaGroups.TryGetValue(cell.Value, out var antennaGroup))
            {
                antennaGroups[cell.Value] = antennaGroup = [];
            }
            antennaGroup.Add(cell.Pos);
        }
        return (grid, antennaGroups);
    }

    private static Pos? FindAntiNode(Pos a, Pos b, char[][] grid) =>
        b + (b - a) is { } antiNode && grid.Contains(antiNode) ? antiNode : null;

    private static IEnumerable<Pos> FindAntiNodes(Pos a, Pos b, char[][] grid)
    {
        while (FindAntiNode(a, b, grid) is { } antiNode)
        {
            yield return antiNode;
            a = b;
            b = antiNode;
        }
    }
}
