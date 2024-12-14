namespace AdventOfCode.Y2024.Day14;

[ProblemName("Restroom Redoubt")]
class Solution : Solver 
{
    private const int Width = 101;
    private const int Height = 103;

    public object PartOne(string[] lines) 
    {
        var robots = Parse(lines).ToArray();

        for (int seconds = 0; seconds < 100; seconds++)
        {
            robots = robots.Select(Move).ToArray();
        }

        (Pos Start, Pos End)[] quadrants = [
            (new(0, 0),              new(Width / 2 - 1, Height / 2 - 1)), (new(Width / 2 + 1, 0),              new(Width - 1, Height / 2 - 1)), 
            (new(0, Height / 2 + 1), new(Width / 2 - 1, Height - 1)),     (new(Width / 2 + 1, Height / 2 + 1), new(Width - 1, Height - 1))
        ];

        return quadrants
            .Select(q => robots.Count(r => r.Pos.Row >= q.Start.Row && r.Pos.Row <= q.End.Row 
                                        && r.Pos.Col >= q.Start.Col && r.Pos.Col <= q.End.Col))
            .Aggregate(1, (safetyFactor, count) => safetyFactor * count);
    }

    public object PartTwo(string[] lines) 
    {
        var robots = Parse(lines).ToArray();
        int seconds = 0;
        while (true)
        {
            robots = robots.Select(Move).ToArray();
            seconds++;
            if (Enumerable.Range(0, Width).Any(col => robots.Count(robots => robots.Pos.Col == col) > 30)
                && Enumerable.Range(0, Height).Any(row => robots.Count(robots => robots.Pos.Row == row) > 30))
            {
                Print(robots);
                return seconds;
            }
        }
    }

    private static IEnumerable<Robot> Parse(string[] lines) =>
        lines.Select(line =>
        {
            var parts = line.Split(["p=", ",", " v="], StringSplitOptions.RemoveEmptyEntries);
            return new Robot(new(int.Parse(parts[0]), int.Parse(parts[1])), new(int.Parse(parts[2]), int.Parse(parts[3])));
        });

    private static Robot Move(Robot robot) =>
        robot with
        {
            Pos = new(
                (((robot.Pos.Col + robot.Vel.Col) % Width) + Width) % Width,
                (((robot.Pos.Row + robot.Vel.Row) % Height) + Height) % Height)
        };

    private static void Print(Robot[] robots)
    {
        for (int row = 0; row < Height; row++)
        {
            for (int col = 0; col < Width; col++)
            {
                if (robots.Any(r => r.Pos.Row == row && r.Pos.Col == col))
                {
                    Console.Write("##");
                }
                else
                {
                    Console.Write("  ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private record struct Robot(Pos Pos, Pos Vel);
}
