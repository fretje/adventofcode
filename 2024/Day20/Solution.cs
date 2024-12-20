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
            .Count(end => distances.TryGetValue(end, out var endDistance) && start.Value - endDistance >= 102));
    }

    public object PartTwo(string[] lines)
    {
        var distances = ParseInput(lines);
        var count = 0;
        foreach (var start in distances)
        {
            foreach (var end in distances)
            {
                var distanceFromStart = Math.Abs(end.Key.Col - start.Key.Col) + Math.Abs(end.Key.Row - start.Key.Row);
                var timeSaved = start.Value - end.Value - distanceFromStart;
                if (distanceFromStart <= 20 && timeSaved >= 100)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private static Dictionary<Pos, int> ParseInput(string[] lines)
    {
        var grid = lines.ToGrid();
        var start = grid.AllCells().Single(c => c.Value == 'S').Pos;
        var end = grid.AllCells().Single(c => c.Value == 'E').Pos;
        var curPos = start;
        Dictionary<Pos, int> distances = [];
        distances[curPos] = 0;
        while (curPos != end)
        {
            var nextPos = Directions.All
                .Select(d => curPos + d)
                .Single(p => grid.Contains(p) && grid.ValueAt(p) != '#' && !distances.ContainsKey(p));
            distances[nextPos] = distances[curPos] + 1;
            curPos = nextPos;
        }
        return distances;
    }
}
