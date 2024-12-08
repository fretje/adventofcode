namespace AdventOfCode.Y2024.Day06;

[ProblemName("Guard Gallivant")]
internal class Solution : Solver
{
    private static char[] _guardChars = ['^', '>', 'v', '<'];

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

    private static (char Value, Pos Pos) GetGuard(char[][] grid) =>
        grid.AllCells().First(c => _guardChars.Contains(c.Value));

    private static Pos[] GetDistinctPositions(char[][] grid, (char Value, Pos Pos) guard) =>
        [.. WalkGuard(guard, grid).Select(p => p.Pos).Distinct()];

    private static int GetObstructionsWithLoopCount(char[][] grid, (char Value, Pos Pos) guard)
    {
        var distinctPositions = GetDistinctPositions(grid, guard);
        var obstructionsWithLoopCount = 0;
        Parallel.For(0, distinctPositions.Length, i =>
        {
            var pos = distinctPositions[i];
            if (pos != guard.Pos)
            {
                var newGrid = grid.DeepClone();
                newGrid.SetValueAt(pos, '#');
                if (HasLoop(newGrid, guard))
                {
                    Interlocked.Increment(ref obstructionsWithLoopCount);
                }
            }
        });
        return obstructionsWithLoopCount;
    }

    private static bool HasLoop(char[][] grid, (char Direction, Pos Pos) guard)
    {
        HashSet<(char, Pos)> visited = [];
        foreach (var pos in WalkGuard(guard, grid))
        {
            if (!visited.Add(pos))
            {
                return true;
            }
        }
        return false;
    }

    private static IEnumerable<(char Direction, Pos Pos)> WalkGuard((char Direction, Pos Pos) current, char[][] grid)
    {
        yield return current;
        while (NextGuardPos(current, grid) is { } next)
        {
            yield return next;
            current = next;
        }
    }

    private static (char Direction, Pos Pos)? NextGuardPos((char Direction, Pos Pos) current, char[][] grid)
    {
        var nextPos = NextPos(current);
        return grid.Contains(nextPos)
            ? grid.ValueAt(nextPos) == '#'
                ? NextGuardPos((NextDirection(current.Direction), current.Pos), grid)
                : (current.Direction, nextPos)
            : null;
    }

    private static Pos NextPos((char Direction, Pos Pos) current) =>
        current.Direction switch
        {
            '^' => new Pos(current.Pos.Col, current.Pos.Row - 1),
            '>' => new Pos(current.Pos.Col + 1, current.Pos.Row),
            'v' => new Pos(current.Pos.Col, current.Pos.Row + 1),
            '<' => new Pos(current.Pos.Col - 1, current.Pos.Row),
            _ => throw new Exception("Invalid direction")
        };

    private static char NextDirection(char direction) =>
        direction switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => throw new Exception("Invalid direction")
        };
}
