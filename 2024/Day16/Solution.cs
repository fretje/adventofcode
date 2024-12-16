namespace AdventOfCode.Y2024.Day16;

[ProblemName("Reindeer Maze")]
class Solution : Solver 
{
    public object PartOne(string[] lines) => Solve(lines);

    public object PartTwo(string[] lines) => Solve(lines, part2: true);

    private static long Solve(string[] lines, bool part2 = false)
    {
        var grid = lines.ToGrid();
        var start = grid.AllCells().First(c => c.Value == 'S').Pos;
        var startDir = Directions.Right;
        var end = grid.AllCells().First(c => c.Value == 'E').Pos;

        List<(long Price, HashSet<Pos> Path)> solutions = [];
        Queue<(Pos Pos, Pos Dir, long Price, HashSet<Pos> Path)> queue = [];
        Dictionary<(Pos, Pos), long> seen = [];

        queue.Enqueue((start, startDir, 0, part2 ? [start] : []));
        seen[(start, startDir)] = 0;
        while (queue.Count != 0)
        {
            var (pos, dir, price, path) = queue.Dequeue();
            if (!grid.Contains(pos) || grid.ValueAt(pos) == '#')
            {
                continue;
            }
            if (pos == end)
            {
                solutions.Add((price, path));
                continue;
            }
            TryEnqueue(pos + dir, dir, price + 1, part2 ? [.. path, pos + dir] : path);
            TryEnqueue(pos, GetDirectionAfterTurn(dir, Directions.Left), price + 1000, path);
            TryEnqueue(pos, GetDirectionAfterTurn(dir, Directions.Right), price + 1000, path);

            void TryEnqueue(Pos pos, Pos dir, long price, HashSet<Pos> path)
            {
                if (seen.TryGetValue((pos, dir), out var seenPrice) && (part2 ? seenPrice < price : seenPrice <= price))
                {
                    return;
                }
                seen[(pos, dir)] = price;
                queue.Enqueue((pos, dir, price, path));
            }
        }

        var min = solutions.Min(s => s.Price);
        if (!part2)
        {
            return min;
        }

        return solutions.Where(s => s.Price == min).SelectMany(s => s.Path).Distinct().Count();
    }

    private static Pos GetDirectionAfterTurn(Pos currentDirection, Pos turn)
    {
        if (currentDirection == Directions.Left)
        {
            return turn == Directions.Left ? Directions.Down : Directions.Up;
        }
        if (currentDirection == Directions.Right)
        {
            return turn == Directions.Left ? Directions.Up : Directions.Down;
        }
        if (currentDirection == Directions.Up)
        {
            return turn == Directions.Left ? Directions.Left : Directions.Right;
        }
        if (currentDirection == Directions.Down)
        {
            return turn == Directions.Left ? Directions.Right : Directions.Left;
        }
        throw new InvalidOperationException();
    }
}
