namespace AdventOfCode;

public record struct Pos(int Col, int Row)
{
    public static Pos operator +(Pos a, Pos b) => new(a.Col + b.Col, a.Row + b.Row);
}
