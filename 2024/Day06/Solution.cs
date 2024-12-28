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

    private static Pose GetGuard(char[][] grid) =>
        new(grid.AllCells().First(c => c.Value == '^').Pos, Directions.Up);

    private static Pos[] GetDistinctPositions(char[][] grid, Pose guard) =>
        [.. WalkGuard(guard, grid).Select(p => p.Pos).Distinct()];

    private static int GetObstructionsWithLoopCount(char[][] grid, Pose guard)
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

    private static bool HasLoop(char[][] grid, Pose guard)
    {
        HashSet<Pose> visited = [];
        foreach (var pose in WalkGuard(guard, grid))
        {
            if (!visited.Add(pose))
            {
                return true;
            }
        }
        return false;
    }

    private static IEnumerable<Pose> WalkGuard(Pose current, char[][] grid)
    {
        yield return current;
        while (NextGuardPose(current, grid) is { } next)
        {
            yield return next;
            current = next;
        }
    }

    private static Pose? NextGuardPose(Pose current, char[][] grid)
    {
        var next = current.Move();
        return grid.Contains(next.Pos)
            ? grid.ValueAt(next.Pos) == '#'
                ? NextGuardPose(current.TurnRight(), grid)
                : next
            : null;
    }
}
