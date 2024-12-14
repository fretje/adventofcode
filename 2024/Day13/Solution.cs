namespace AdventOfCode.Y2024.Day13;

[ProblemName("Claw Contraption")]
class Solution : Solver 
{
    public object PartOne(string[] lines) =>
        ParseInput(lines)
            .Select(FindSolutionBruteForce)
            .Where(s => s.HasValue).Select(s => s!.Value)
            .Sum(s => s.APresses * 3 + s.BPresses);

    public object PartTwo(string[] lines) =>
        ParseInput(lines)
            .Select(claw => claw with { Prize = new(claw.Prize.X + 10_000_000_000_000, claw.Prize.Y + 10_000_000_000_000) })
            .Select(FindSolution)
            .Where(s => s.HasValue).Select(s => s!.Value)
            .Sum(s => s.APresses * 3 + s.BPresses);

    private static IEnumerable<Claw> ParseInput(string[] lines) => 
        lines.Chunk(4).Select<string[], Claw>(chunk =>
        {
            var a = chunk[0].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('+')[1])).ToArray();
            var b = chunk[1].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('+')[1])).ToArray();
            var p = chunk[2].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('=')[1])).ToArray();
            return new(new(a[0], a[1]), new(b[0], b[1]), new(p[0], p[1]));
        });

    private static (long APresses, long BPresses)? FindSolutionBruteForce(Claw claw)
    {
        for (long aPresses = 0; aPresses <= 100; aPresses++)
        {
            for (long bPresses = 0; bPresses <= 100; bPresses++)
            {
                if ((aPresses * claw.A.X) + (bPresses * claw.B.X) == claw.Prize.X 
                    && (aPresses * claw.A.Y) + (bPresses * claw.B.Y) == claw.Prize.Y)
                {
                    return (aPresses, bPresses);
                }
            }
        }
        return null;
    }

    private static (long APresses, long BPresses)? FindSolution(Claw claw)
    {
        var (a, b, prize) = claw;
        var aPresses = (double)(prize.X * b.Y - prize.Y * b.X) / (a.X * b.Y - a.Y * b.X);
        var bPresses = (double)(prize.X - a.X * aPresses) / b.X;
        if (aPresses % 1 != 0 || bPresses % 1 != 0)
        {
            return null;
        }
        return ((long)aPresses, (long)bPresses);
    }

    private record struct Claw(Coords A, Coords B, Coords Prize);
    private record struct Coords(long X, long Y);
}
