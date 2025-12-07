namespace AdventOfCode.Y2025.Day07;

[ProblemName("Laboratories")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var grid = lines.ToGrid();
        var splitCount = 0;
        for (int row = 1; row < grid.Length; row++)
        {
            for (int col = 0; col < grid[0].Length; col++)
            {
                if (grid[row-1][col] is 'S' or '|')
                {
                    if (grid[row][col] == '.')
                    {
                        grid[row][col] = '|';
                    }
                    else if (grid[row][col] == '^')
                    {
                        grid[row][col - 1] = '|';
                        grid[row][col + 1] = '|';
                        splitCount++;
                    }
                }
            }
        }
        return splitCount;
    }

    public object PartTwo(string[] lines) =>
        GetTimelineCount(new(lines[0].IndexOf('S'), 0), lines.ToGrid());

    private static long GetTimelineCount(Pos pos, char[][] grid, Dictionary<Pos, long>? memo = null)
    {
        memo ??= [];
        if (memo.TryGetValue(pos, out var value))
        {
            return value;
        }
        if (pos.Row == grid.Length - 1)
        {
            return 1;
        }
        var timelineCount = 0L;
        Pos nextPos = new(pos.Col, pos.Row + 1);
        if (grid.ValueAt(nextPos) is '.')
        {
            timelineCount += GetTimelineCount(nextPos, grid, memo);
        }
        else if (grid.ValueAt(nextPos) is '^')
        {
            timelineCount += GetTimelineCount(nextPos + Directions.Left, grid, memo);
            timelineCount += GetTimelineCount(nextPos + Directions.Right, grid, memo);
        }
        memo[pos] = timelineCount;
        return timelineCount;
    }
}
