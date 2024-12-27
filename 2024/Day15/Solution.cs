namespace AdventOfCode.Y2024.Day15;

[ProblemName("Warehouse Woes")]
class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var (grid, moves) = ParseInput(lines);
        ExecuteMoves(grid, moves);
        return grid.AllCells().Where(c => c.Value == 'O').Sum(c => c.Pos.Row * 100 + c.Pos.Col);
    }

    public object PartTwo(string[] lines)
    {
        var (grid, moves) = ParseInput(lines, true);
        ExecuteMoves(grid, moves);
        return grid.AllCells().Where(c => c.Value == '[').Sum(c => c.Pos.Row * 100 + c.Pos.Col);
    }

    private static (char[][] Grid, Pos[] Moves) ParseInput(string[] lines, bool part2 = false)
    {
        List<char[]> grid = [];
        List<Pos> moves = [];
        bool readingGrid = true;
        foreach (var line in lines)
        {
            if (readingGrid)
            {
                if (line == "")
                {
                    readingGrid = false;
                    continue;
                }
                grid.Add(part2 
                    ?  [.. line.ToCharArray().SelectMany((c) => c switch
                        {
                            '#' => (char[])['#', '#'],
                            'O' => ['[', ']'],
                            '.' => ['.', '.'],
                            '@' => ['@', '.'],
                            _ => throw new InvalidOperationException(),
                        })] 
                    : line.ToCharArray());
                continue;
            }
            moves.AddRange(line.ToCharArray().Select(Directions.FromChar));
        }
        return (grid.ToArray(), moves.ToArray());
    }

    private static void ExecuteMoves(char[][] grid, Pos[] moves)
    {
        var robot = grid.AllCells().Single(c => c.Value == '@').Pos;
        foreach (var move in moves)
        {
            HashSet<Pos> positionsToMove = GetPositionsToMove(robot, move, grid);
            if (positionsToMove.Count == 0)
            {
                continue;
            }
            var copy = grid.DeepClone();
            foreach (var pos in positionsToMove)
            {
                grid.SetValueAt(pos, '.');
            }
            foreach (var pos in positionsToMove)
            {
                grid.SetValueAt(pos + move, copy.ValueAt(pos));
            }
            robot += move;
        }
    }

    static HashSet<Pos> GetPositionsToMove(Pos start, Pos direction, char[][] grid)
    {
        HashSet<Pos> positions = [start];
        HashSet<Pos> nextPositions = [start];
        while (true)
        {
            HashSet<Pos> newNextPositions = [];
            foreach (var pos in nextPositions)
            {
                var nextPos = pos + direction;
                if (positions.Contains(nextPos))
                {
                    continue;
                }
                var nextValue = grid.ValueAt(nextPos);
                if (nextValue == '#')
                {
                    return [];
                }
                if ("O[]".Contains(nextValue))
                {
                    newNextPositions.Add(nextPos);
                    if (nextValue == '[')
                    {
                        newNextPositions.Add(nextPos + Directions.Right);
                    }
                    else if (nextValue == ']')
                    {
                        newNextPositions.Add(nextPos + Directions.Left);
                    }
                }
            }
            if (newNextPositions.Count == 0)
            {
                return positions;
            }
            positions.UnionWith(newNextPositions);
            nextPositions = newNextPositions;
        }
    }
}
