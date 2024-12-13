namespace AdventOfCode.Y2024.Day12;

[ProblemName("Garden Groups")]
class Solution : Solver
{
    public object PartOne(string[] lines) =>
        lines.ToGrid().GetRegions().Sum(r => r.Count * r.GetPerimeter());

    public object PartTwo(string[] lines) =>
        lines.ToGrid().GetRegions().Sum(r => r.Count * r.GetSides());
}

public static class Extensions
{
    public static IEnumerable<HashSet<Pos>> GetRegions(this char[][] grid)
    {
        List<HashSet<Pos>> regions = [];
        foreach (var (_, pos) in grid.AllCells())
        {
            if (regions.Any(r => r.Contains(pos)))
            {
                continue;
            }
            var region = GetRegion(pos, grid);
            regions.Add(region);
            yield return region;
        }
    }

    private static HashSet<Pos> GetRegion(Pos pos, char[][] grid)
    {
        HashSet<Pos> region = [pos];
        HashSet<Pos> next = [.. region];
        while (true)
        {
            HashSet<Pos> newNext = [];
            foreach (var p in next)
            {
                foreach (var d in _directions)
                {
                    var newPos = new Pos(p.Col + d.Col, p.Row + d.Row);
                    if (!region.Contains(newPos) && grid.Contains(newPos) && grid.ValueAt(newPos) == grid.ValueAt(pos))
                    {
                        newNext.Add(newPos);
                        region.Add(newPos);
                    }
                }
            }
            if (newNext.Count == 0)
            {
                return region;
            }
            next = newNext;
        }
    }

    public static int GetPerimeter(this HashSet<Pos> region) => GetPerimeters(region).Count();

    private static IEnumerable<(Pos Pos, Pos Dir)> GetPerimeters(HashSet<Pos> region)
    {
        foreach (var pos in region)
        {
            foreach (var dir in _directions)
            {
                var newPos = new Pos(pos.Col + dir.Col, pos.Row + dir.Row);
                if (!region.Contains(newPos))
                {
                    yield return (pos, dir);
                }
            }
        }
    }

    public static int GetSides(this HashSet<Pos> region)
    {
        var perimeters = GetPerimeters(region).GroupBy(per => per.Dir)
            .ToDictionary(per => per.Key, per => per.Select(p => p.Pos)
                                                    .OrderBy(p => per.Key == new Pos(1, 0) || per.Key == new Pos(-1, 0) ? p.Row : p.Col).ToList());
        foreach (var (dir, pers) in perimeters)
        {
            var nextDir = dir == new Pos(1, 0) || dir == new Pos(-1, 0) ? new Pos(0, -1) : new Pos(-1, 0);
            for (int i = pers.Count - 1; i >= 0; i--)
            {
                var per = pers[i];
                if (pers.Contains(new Pos(per.Col + nextDir.Col, per.Row + nextDir.Row)))
                {
                    pers.RemoveAt(i);
                }
            }
        }
        return perimeters.Values.Sum(p => p.Count);
    }

    private static readonly Pos[] _directions = [
        new Pos(1, 0), // right
        new Pos(0, 1), // down
        new Pos(-1, 0), // left
        new Pos(0, -1), // up
    ];
}
