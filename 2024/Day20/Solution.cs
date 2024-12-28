namespace AdventOfCode.Y2024.Day20;

[ProblemName("Race Condition")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var distances = ParseInput(lines);
        return distances.Sum(start => ((Pos[])[
                start.Key + Directions.Up    + Directions.Up,    start.Key + Directions.Up    + Directions.Right,
                start.Key + Directions.Right + Directions.Right, start.Key + Directions.Right + Directions.Down,
                start.Key + Directions.Down  + Directions.Down,  start.Key + Directions.Down  + Directions.Left,
                start.Key + Directions.Left  + Directions.Left,  start.Key + Directions.Left  + Directions.Up,])
            .Count(end => distances.TryGetValue(end, out var endDist) && start.Value - endDist >= 102));
    }

    public object PartTwo(string[] lines)
    {
        var distances = ParseInput(lines);
        var count = 0;
        Parallel.ForEach(distances, start =>
        {
            var (startPos, startDist) = start;
            foreach (var (endPos, endDist) in distances)
            {
                var distFromStart = Math.Abs(endPos.Col - startPos.Col) + Math.Abs(endPos.Row - startPos.Row);
                if (distFromStart <= 20 && startDist - endDist - distFromStart >= 100)
                {
                    Interlocked.Increment(ref count);
                }
            }
        });
        return count;
    }

    private static Dictionary<Pos, int> ParseInput(string[] lines)
    {
        var grid = lines.ToGrid();
        var curPos = grid.AllCells().Single(c => c.Value == 'S').Pos;
        var endPos = grid.AllCells().Single(c => c.Value == 'E').Pos;
        Dictionary<Pos, int> distances = [];
        distances[curPos] = 0;
        while (curPos != endPos)
        {
            var nextPos = Directions.Othogonal
                .Select(dir => curPos + dir)
                .Single(pos => grid.Contains(pos) && grid.ValueAt(pos) != '#' && !distances.ContainsKey(pos));
            distances[nextPos] = distances[curPos] + 1;
            curPos = nextPos;
        }
        return distances;
    }
}
