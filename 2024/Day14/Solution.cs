namespace AdventOfCode.Y2024.Day14;

[ProblemName("Restroom Redoubt")]
class Solution : Solver 
{
    private const int Width = 101;
    private const int Height = 103;

    private static readonly (Pos Start, Pos End)[] Quadrants = [
        (new(0, 0),              new(Width / 2 - 1, Height / 2 - 1)), (new(Width / 2 + 1, 0),              new(Width - 1, Height / 2 - 1)), 
        (new(0, Height / 2 + 1), new(Width / 2 - 1, Height - 1)),     (new(Width / 2 + 1, Height / 2 + 1), new(Width - 1, Height - 1))
    ];

    public object PartOne(string[] lines) => GetSafetyFactor(Parse(lines).Select(r => Move(r, 100)));

    public object PartTwo(string[] lines) 
    {
        var robots = Parse(lines).ToArray();
        int seconds = 0;
        while (true)
        {
            robots = robots.Select(r => Move(r, 1)).ToArray();
            seconds++;
            var safetyFactor = GetSafetyFactor(robots);
            if (safetyFactor < 110_000_000)
            {
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

    private static Robot Move(Robot robot, int count) =>
        robot with
        {
            Pos = new(
                (((robot.Pos.Col + robot.Vel.Col * count) % Width) + Width) % Width,
                (((robot.Pos.Row + robot.Vel.Row * count) % Height) + Height) % Height)
        };

    private static int GetSafetyFactor(IEnumerable<Robot> robots) =>
        Quadrants
            .Select(q => robots.Count(r => r.Pos.Row >= q.Start.Row && r.Pos.Row <= q.End.Row
                                        && r.Pos.Col >= q.Start.Col && r.Pos.Col <= q.End.Col))
            .Aggregate(1, (safetyFactor, count) => safetyFactor * count);

    private record struct Robot(Pos Pos, Pos Vel);
}
