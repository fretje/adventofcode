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
        Queue<Pos> queue = [];
        queue.Enqueue(pos);
        while (queue.Count > 0) 
        {
            var currentPos = queue.Dequeue();
            foreach (var nextPos in Directions.Othogonal
                .Select(dir => currentPos + dir)
                .Where(nextPos => grid.Contains(nextPos) && grid.ValueAt(nextPos) == grid.ValueAt(pos) && region.Add(nextPos)))
            {
                queue.Enqueue(nextPos);
            }
        }
        return region;
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
        var sides = 0;
        foreach (var (dir, perimeters) in GetPerimeters(region).GroupBy(per => per.Dir).Select(g =>
            (g.Key == Directions.Left || g.Key == Directions.Right ? Directions.Up : Directions.Left, 
             g.Select(per => per.Pos).OrderBy(pos => g.Key == Directions.Left || g.Key == Directions.Right ? pos.Row : pos.Col).ToList())))
        {
            for (int i = perimeters.Count - 1; i >= 0; i--)
            {
                if (perimeters.Contains(perimeters[i] + dir))
                {
                    perimeters.RemoveAt(i);
                }
            }
            sides += perimeters.Count;
        }
        return sides;
    }
}
