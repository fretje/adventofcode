namespace AdventOfCode;

public record struct Pos(int Col, int Row)
{
    public static Pos operator +(Pos a, Pos b) => new(a.Col + b.Col, a.Row + b.Row);
    public static Pos operator -(Pos a, Pos b) => new(a.Col - b.Col, a.Row - b.Row);
}

public record struct Pos3D(int X, int Y, int Z)
{
    public readonly double DistanceTo(Pos3D other) =>
        Math.Sqrt(Math.Pow(other.X - X, 2) + Math.Pow(other.Y - Y, 2) + Math.Pow(other.Z - Z, 2));
}
