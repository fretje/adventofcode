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
                foreach (var d in Directions.Othogonal)
                {
                    var newPos = p + d;
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
            foreach (var dir in Directions.Othogonal)
            {
                if (!region.Contains(pos + dir))
                {
                    yield return (pos, dir);
                }
            }
        }
    }

    public static int GetSides(this HashSet<Pos> region)
    {
        var perimetersPerDirection = GetPerimeters(region).GroupBy(per => per.Dir).ToDictionary(
            perimeters => perimeters.Key, 
            perimeters => perimeters.Select(per => per.Pos)
                .OrderBy(pos => perimeters.Key == Directions.Left || perimeters.Key == Directions.Right ? pos.Row : pos.Col).ToList());
        foreach (var (dir, perimeters) in perimetersPerDirection)
        {
            var nextDir = dir == Directions.Left || dir == Directions.Right ? Directions.Up : Directions.Left;
            for (int i = perimeters.Count - 1; i >= 0; i--)
            {
                if (perimeters.Contains(perimeters[i] + nextDir))
                {
                    perimeters.RemoveAt(i);
                }
            }
        }
        return perimetersPerDirection.Values.Sum(per => per.Count);
    }
}
