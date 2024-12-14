namespace AdventOfCode.Y2024.Day14;

[ProblemName("Restroom Redoubt")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var robots = Parse(lines).ToArray();

        var width = 101;
        var height = 103;

        for (int seconds = 0; seconds < 100; seconds++)
        {
            robots = robots.Select(r => Move(r, width, height)).ToArray();
        }

        var safetyFactor = 0;
        List<(Pos, Pos)> quadrants = [
            (new(0, 0),              new(width / 2 - 1, height / 2 - 1)), (new(width / 2 + 1, 0),              new(width - 1, height / 2 - 1)), 
            (new(0, height / 2 + 1), new(width / 2 - 1, height - 1)),     (new(width / 2 + 1, height / 2 + 1), new(width - 1, height - 1))];
        foreach (var (start, end) in quadrants)
        {
            var count = robots.Count(r => r.Pos.Row >= start.Row && r.Pos.Row <= end.Row && r.Pos.Col >= start.Col && r.Pos.Col <= end.Col);
            safetyFactor = safetyFactor == 0 ? count : safetyFactor * count;
            Console.WriteLine($"Quadrant {start} - {end}: {count}");
        }

        return safetyFactor;
    }

    public object PartTwo(string[] lines) 
    {
        var robots = Parse(lines).ToArray();

        var width = 101;
        var height = 103;

        int seconds = 0;
        while (true)
        {
            robots = robots.Select(r => Move(r, width, height)).ToArray();
            seconds++;
            if (Enumerable.Range(0, width).Any(col => robots.Count(robots => robots.Pos.Col == col) > 30)
                && Enumerable.Range(0, height).Any(row => robots.Count(robots => robots.Pos.Row == row) > 30))
            {
                Console.WriteLine("Seconds: " + seconds);
                Print(robots, width, height);
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

    private static Robot Move(Robot robot, int gridWidth, int gridHeight) =>
        robot with
        {
            Pos = new(
                (((robot.Pos.Col + robot.Vel.Col) % gridWidth) + gridWidth) % gridWidth,
                (((robot.Pos.Row + robot.Vel.Row) % gridHeight) + gridHeight) % gridHeight)
        };

    private static void Print(Robot[] robots, int gridWidth, int gridHeight)
    {
        for (int row = 0; row < gridHeight; row++)
        {
            for (int col = 0; col < gridWidth; col++)
            {
                var count = robots.Count(r => r.Pos.Row == row && r.Pos.Col == col);
                if (count > 0)
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
