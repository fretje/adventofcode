namespace AdventOfCode.Y2025.Day07;

[ProblemName("Laboratories")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var grid = lines.ToGrid();
        var splitCount = 0;
        for (var row = 1; row < grid.Length; row++)
        {
            for (var col = 0; col < grid[0].Length; col++)
            {
                if (grid[row-1][col] is 'S' or '|')
                {
                    if (grid[row][col] is '^')
                    {
                        grid[row][col - 1] = '|';
                        grid[row][col + 1] = '|';
                        splitCount++;
                    }
                    else
                    {
                        grid[row][col] = '|';
                    }
                }
            }
        }
        return splitCount;
    }

    public object PartTwo(string[] lines) =>
        GetTimelineCount(new(lines[0].IndexOf('S'), 0), lines.ToGrid(), []);

    private static long GetTimelineCount(Pos pos, char[][] grid, Dictionary<Pos, long> memo)
    {
        if (memo.TryGetValue(pos, out var value))
        {
            return value;
        }
        if (pos.Row == grid.Length - 1)
        {
            return 1;
        }
        var nextPos = pos + Directions.Down;
        var timelineCount = grid.ValueAt(nextPos) is '^'
            ? GetTimelineCount(nextPos + Directions.Left, grid, memo) + GetTimelineCount(nextPos + Directions.Right, grid, memo)
            : GetTimelineCount(nextPos, grid, memo);
        memo[pos] = timelineCount;
        return timelineCount;
    }
}
