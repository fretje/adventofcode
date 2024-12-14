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
        lines.Chunk(4).Select(chunk =>
        {
            var a = chunk[0].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('+')[1])).ToArray();
            var b = chunk[1].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('+')[1])).ToArray();
            var p = chunk[2].Split(": ")[1].Split(", ").Select(x => long.Parse(x.Split('=')[1])).ToArray();
            return new Claw(new(a[0], a[1]), new(b[0], b[1]), new(p[0], p[1]));
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
        var (a, b, p) = claw;

        // na * a.X + nb * b.X = p.X  ==>  nb = (p.X - na * a.X) / b.X
        // na * a.Y + nb * b.Y = p.Y  ==>  na = (p.Y - nb * b.Y) / a.Y

        // na = (p.Y - ((p.X - na * a.X) / b.X) * b.Y) / a.Y
        // na = (p.Y - (p.X * b.Y - na * a.X * b.Y) / b.X) / a.Y
        // na = (p.Y * b.X - p.X * b.Y + na * a.X * b.Y) / (a.Y * b.X)
        // na * (a.Y * b.X) = p.Y * b.X - p.X * b.Y + na * a.X * b.Y
        // na * (a.Y * b.X) - na * a.X * b.Y = p.Y * b.X - p.X * b.Y
        // na * (a.Y * b.X - a.X * b.Y) = p.Y * b.X - p.X * b.Y
        // na = (p.Y * b.X - p.X * b.Y) / (a.Y * b.X - a.X * b.Y)

        var na = (double)(p.Y * b.X - p.X * b.Y) / (a.Y * b.X - a.X * b.Y);
        var nb = (double)(p.X - na * a.X) / b.X;
        if (na % 1 != 0 || nb % 1 != 0)
        {
            return null;
        }
        return ((long)na, (long)nb);
    }

    private record struct Claw(Coords A, Coords B, Coords Prize);
    private record struct Coords(long X, long Y);
}
