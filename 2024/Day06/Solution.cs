namespace AdventOfCode.Y2024.Day06;

[ProblemName("Guard Gallivant")]
internal class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var grid = lines.ToGrid();
        return GetDistinctPositions(grid, GetGuard(grid)).Length;
    }

    public object PartTwo(string[] lines)
    {
        var grid = lines.ToGrid();
        return GetObstructionsWithLoopCount(grid, GetGuard(grid));
    }

    private static (Pos Pos, Pos Dir) GetGuard(char[][] grid) =>
        (grid.AllCells().First(c => c.Value == '^').Pos, Directions.Up);

    private static Pos[] GetDistinctPositions(char[][] grid, (Pos Pos, Pos Dir) guard) =>
        [.. WalkGuard(guard, grid).Select(p => p.Pos).Distinct()];

    private static int GetObstructionsWithLoopCount(char[][] grid, (Pos Pos, Pos Dir) guard)
    {
        var distinctPositions = GetDistinctPositions(grid, guard);
        var obstructionsWithLoopCount = 0;
        Parallel.ForEach(distinctPositions.Where(pos => pos != guard.Pos), pos =>
        {
            var newGrid = grid.DeepClone();
            newGrid.SetValueAt(pos, '#');
            if (HasLoop(newGrid, guard))
            {
                Interlocked.Increment(ref obstructionsWithLoopCount);
            }
        });
        return obstructionsWithLoopCount;
    }

    private static bool HasLoop(char[][] grid, (Pos Pos, Pos Dir) guard)
    {
        HashSet<(Pos, Pos)> visited = [];
        foreach (var pos in WalkGuard(guard, grid))
        {
            if (!visited.Add(pos))
            {
                return true;
            }
        }
        return false;
    }

    private static IEnumerable<(Pos Pos, Pos Dir)> WalkGuard((Pos Pos, Pos Dir) current, char[][] grid)
    {
        yield return current;
        while (NextGuardPos(current, grid) is { } next)
        {
            yield return next;
            current = next;
        }
    }

    private static (Pos Pos, Pos Dir)? NextGuardPos((Pos Pos, Pos Dir) current, char[][] grid)
    {
        var nextPos = current.Pos + current.Dir;
        return grid.Contains(nextPos)
            ? grid.ValueAt(nextPos) == '#'
                ? NextGuardPos((current.Pos, Directions.TurnRight(current.Dir)), grid)
                : (nextPos, current.Dir)
            : null;
    }
}
