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
                if (part2)
                {
                    grid.Add([.. line.ToCharArray().SelectMany(c => c switch
                    {
                        '#' => "##".ToCharArray(),
                        'O' => "[]".ToCharArray(),
                        '.' => "..".ToCharArray(),
                        '@' => "@.".ToCharArray(),
                        _ => throw new InvalidOperationException(),
                    })]);
                    continue;
                }
                grid.Add(line.ToCharArray());
                continue;
            }
            moves.AddRange(line.ToCharArray().Select(Directions.FromChar));
        }
        return (grid.ToArray(), moves.ToArray());
    }

    private static void ExecuteMoves(char[][] grid, Pos[] moves)
    {
        var robot = grid.AllCells().Where(c => c.Value == '@').Single().Pos;
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

//lines = [
//    "#######",
//    "#...#.#",
//    "#.....#",
//    "#..OO@#",
//    "#..O..#",
//    "#.....#",
//    "#######",
//    "",
//    "<vv<<^^<<^^",
//];

//lines = [
//    "##########",
//    "#..O..O.O#",
//    "#......O.#",
//    "#.OO..O.O#",
//    "#..O@..O.#",
//    "#O#..O...#",
//    "#O..O..O.#",
//    "#.OO.O.OO#",
//    "#....O...#",
//    "##########",
//    "",
//    "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
//    "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
//    "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
//    "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
//    "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
//    "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
//    ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
//    "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
//    "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
//    "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
//];

/* Also works... but is a bit cumbersome ;-)
    private static bool TryMove(Pos currentPos, Pos direction, char[][] grid, out Pos newPos)
    {
        newPos = currentPos;
        var nextPos = currentPos + direction;
        var nextValue = grid.ValueAt(nextPos);
        if (nextValue is '#')
        {
            return false;
        }

        if (nextValue is '.')
        {
            var currentValue = grid.ValueAt(currentPos);
            grid.SetValueAt(currentPos, '.');
            grid.SetValueAt(nextPos, currentValue);
            newPos = nextPos;
            return true;
        }
        else if (nextValue is 'O')
        {
            if (!TryMove(nextPos, direction, grid, out _))
            {
                return false;
            }
            return TryMove(currentPos, direction, grid, out newPos);
        }
        else if (nextValue is '[')
        {
            if (!TryMoveBigBox([nextPos, nextPos + Directions.Right], direction, grid))
            {
                return false;
            }
            return TryMove(currentPos, direction, grid, out newPos);
        }
        else if (nextValue is ']')
        {
            if (!TryMoveBigBox([nextPos + Directions.Left, nextPos], direction, grid))
            {
                return false;
            }
            return TryMove(currentPos, direction, grid, out newPos);
        }

        throw new UnreachableException();
    }

    private static bool TryMoveBigBox(Pos[] item, Pos direction, char[][] grid)
    {
        if (!CanMoveBigBox(item, direction, grid))
        {
            return false;
        }
        if (direction == Directions.Left)
        {
            var nextValue = grid.ValueAt(item[0] + Directions.Left);
            if (nextValue is '#')
            {
                return false;
            }
            if (nextValue is '.')
            {
                grid.SetValueAt(item[0] + Directions.Left, '[');
                grid.SetValueAt(item[1] + Directions.Left, ']');
                grid.SetValueAt(item[1], '.');
                return true;
            }
            if (nextValue is ']')
            {
                return TryMoveBigBox([item[0] + Directions.Left + Directions.Left, item[1] + Directions.Left + Directions.Left], Directions.Left, grid);
            }
        }
        else if (direction == Directions.Right)
        {
            var nextValue = grid.ValueAt(item[1] + Directions.Right);
            if (nextValue is '#')
            {
                return false;
            }
            if (nextValue is '.')
            {
                grid.SetValueAt(item[0] + Directions.Right, '[');
                grid.SetValueAt(item[1] + Directions.Right, ']');
                grid.SetValueAt(item[0], '.');
                return true;
            }
            if (nextValue is '[')
            {
                return TryMoveBigBox([item[0] + Directions.Right + Directions.Right, item[1] + Directions.Right + Directions.Right], Directions.Right, grid);
            }
        }

        var nextPosLeft = item[0] + direction;
        var nextPosRight = item[1] + direction;
        var nextValueLeft = grid.ValueAt(nextPosLeft);
        var nextValueRight = grid.ValueAt(nextPosRight);

        if (nextValueLeft is '#' || nextValueRight is '#')
        {
            return false;
        }

        if (nextValueLeft is '.' && nextValueRight is '.')
        {
            grid.SetValueAt(nextPosLeft, '[');
            grid.SetValueAt(nextPosRight, ']');
            grid.SetValueAt(item[0], '.');
            grid.SetValueAt(item[1], '.');
            return true;
        }

        if (nextValueLeft is '[')
        {
            return TryMoveBigBox([nextPosLeft, nextPosRight], direction, grid);
        }
        if (nextValueLeft is ']' && nextValueRight is '[')
        {
            return TryMoveBigBox([nextPosLeft + Directions.Left, nextPosLeft], direction, grid)
                && TryMoveBigBox([nextPosRight, nextPosRight + Directions.Right], direction, grid);
        }
        if (nextValueLeft is ']')
        {
            return TryMoveBigBox([nextPosLeft + Directions.Left, nextPosLeft], direction, grid);
        }
        if (nextValueRight is '[')
        {
            return TryMoveBigBox([nextPosRight, nextPosRight + Directions.Right], direction, grid);
        }

        throw new UnreachableException();
    }

    private static bool CanMoveBigBox(Pos[] item, Pos direction, char[][] grid)
    {
        if (direction == Directions.Left)
        {
            var nextValue = grid.ValueAt(item[0] + Directions.Left);
            if (nextValue is '#')
            {
                return false;
            }
            if (nextValue is '.')
            {
                return true;
            }
            if (nextValue is ']')
            {
                return CanMoveBigBox([item[0] + Directions.Left + Directions.Left, item[1] + Directions.Left + Directions.Left], Directions.Left, grid);
            }
        }
        else if (direction == Directions.Right)
        {
            var nextValue = grid.ValueAt(item[1] + Directions.Right);
            if (nextValue is '#')
            {
                return false;
            }
            if (nextValue is '.')
            {
                return true;
            }
            if (nextValue is '[')
            {
                return CanMoveBigBox([item[0] + Directions.Right + Directions.Right, item[1] + Directions.Right + Directions.Right], Directions.Right, grid);
            }
        }
        var nextPosLeft = item[0] + direction;
        var nextPosRight = item[1] + direction;
        var nextValueLeft = grid.ValueAt(nextPosLeft);
        var nextValueRight = grid.ValueAt(nextPosRight);
        if (nextValueLeft is '#' || nextValueRight is '#')
        {
            return false;
        }
        if (nextValueLeft is '.' && nextValueRight is '.')
        {
            return true;
        }
        if (nextValueLeft is '[')
        {
            return CanMoveBigBox([nextPosLeft, nextPosRight], direction, grid);
        }
        if (nextValueLeft is ']' && nextValueRight is '[')
        {
            return CanMoveBigBox([nextPosLeft + Directions.Left, nextPosLeft], direction, grid)
                && CanMoveBigBox([nextPosRight, nextPosRight + Directions.Right], direction, grid);
        }
        if (nextValueLeft is ']')
        {
            return CanMoveBigBox([nextPosLeft + Directions.Left, nextPosLeft], direction, grid);
        }
        if (nextValueRight is '[')
        {
            return CanMoveBigBox([nextPosRight, nextPosRight + Directions.Right], direction, grid);
        }
        throw new UnreachableException();
    }
*/
