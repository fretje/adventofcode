namespace AdventOfCode.Y2024.Day16;

[ProblemName("Reindeer Maze")]
class Solution : Solver 
{
    public object PartOne(string[] lines) => Solve(lines);

    public object PartTwo(string[] lines) => Solve(lines, part2: true);

    private static long Solve(string[] lines, bool part2 = false)
    {
        var grid = lines.ToGrid();
        Pose start = new(grid.AllCells().First(c => c.Value == 'S').Pos, Directions.Right);
        Pos end = grid.AllCells().First(c => c.Value == 'E').Pos;

        PriorityQueue<(Pose, long), long> pQueue = new();
        Dictionary<Pose, long> minPrices = [];
        long minimumPrice = long.MaxValue;
        Dictionary<Pose, HashSet<Pose>> backTrack = [];
        HashSet<Pose> endPoses = [];

        pQueue.Enqueue((start, 0), 0);
        minPrices[start] = 0;
        while (pQueue.Count != 0)
        {
            var (pose, price) = pQueue.Dequeue();
            if (minPrices.TryGetValue(pose, out var minPrice) && price > minPrice)
            {
                continue;
            }
            if (pose.Pos == end)
            {
                if (price > minimumPrice)
                {
                    break;
                }
                minimumPrice = price;
                endPoses.Add(pose);
                if (!part2) break;
            }
            TryEnqueue(pose.Move(), price + 1);
            TryEnqueue(pose.TurnLeft(), price + 1000);
            TryEnqueue(pose.TurnRight(), price + 1000);

            void TryEnqueue(Pose newPose, long newPrice)
            {
                if (grid.ValueAt(newPose.Pos) == '#')
                {
                    return;
                }
                var minPrice = minPrices.GetValueOrDefault(newPose, long.MaxValue);
                if (part2 ? newPrice > minPrice : newPrice >= minPrice)
                {
                    return;
                }
                if (newPrice < minPrice)
                {
                    minPrices[newPose] = newPrice;
                    backTrack[newPose] = [];
                }
                backTrack[newPose].Add(pose);
                pQueue.Enqueue((newPose, newPrice), newPrice);
            }
        }

        if (!part2)
        {
            return minimumPrice;
        }

        Queue<Pose> queue = new(endPoses);
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (backTrack.TryGetValue(current, out var previousPoses))
            {
                foreach (var previous in previousPoses)
                {
                    if (endPoses.Add(previous))
                    {
                        queue.Enqueue(previous);                    
                    }
                }
            }
        }
        return endPoses.Select(x => x.Pos).ToHashSet().Count;
    }
}
