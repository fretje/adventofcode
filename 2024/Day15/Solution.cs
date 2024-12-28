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
            var cellsToMove = GetCellsToMove(robot, move, grid);
            if (cellsToMove.Count == 0)
            {
                continue;
            }
            foreach (var cell in cellsToMove)
            {
                grid.SetValueAt(cell.Pos, '.');
            }
            foreach (var cell in cellsToMove)
            {
                grid.SetValueAt(cell.Pos + move, cell.Value);
            }
            robot += move;
        }
    }

    static HashSet<GridCell<char>> GetCellsToMove(Pos start, Pos direction, char[][] grid)
    {
        HashSet<GridCell<char>> cellsToMove = [grid.CellAt(start)];
        HashSet<Pos> seen = [start];
        Queue<Pos> queue = [];
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            var nextPos = currentPos + direction;
            if (seen.Contains(nextPos))
            {
                continue;
            }
            seen.Add(nextPos);
            var nextValue = grid.ValueAt(nextPos);
            if (nextValue is '#')
            {
                return [];
            }
            if ("O[]".Contains(nextValue))
            {
                cellsToMove.Add(grid.CellAt(nextPos));
                queue.Enqueue(nextPos);
                if (nextValue is '[')
                {
                    cellsToMove.Add(grid.CellAt(nextPos + Directions.Right));
                    queue.Enqueue(nextPos + Directions.Right);
                }
                else if (nextValue is ']')
                {
                    cellsToMove.Add(grid.CellAt(nextPos + Directions.Left));
                    queue.Enqueue(nextPos + Directions.Left);
                }
            }
        }
        return cellsToMove;
    }
}
