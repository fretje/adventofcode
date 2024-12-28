namespace AdventOfCode;

public record struct Pose(Pos Pos, Pos Dir)
{
    public readonly Pose Move() => new(Pos + Dir, Dir);
    public readonly Pose TurnLeft() => new(Pos, Directions.TurnLeft(Dir));
    public readonly Pose TurnRight() => new(Pos, Directions.TurnRight(Dir));
}
