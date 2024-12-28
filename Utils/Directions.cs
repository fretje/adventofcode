namespace AdventOfCode;

public static class Directions
{
    public static readonly Pos Right = new(1, 0);
    public static readonly Pos Down = new(0, 1);
    public static readonly Pos Left = new(-1, 0);
    public static readonly Pos Up = new(0, -1);

    public static readonly Pos[] Othogonal = [Right, Down, Left, Up];

    public static readonly Pos UpLeft = new(-1, -1);
    public static readonly Pos UpRight = new(1, -1);
    public static readonly Pos DownRight = new(1, 1);
    public static readonly Pos DownLeft = new(-1, 1);

    public static readonly Pos[] Diagonal = [UpLeft, UpRight, DownRight, DownLeft];

    public static Pos FromChar(char c) => c switch
    {
        '<' => Left,
        '>' => Right,
        '^' => Up,
        'v' => Down,
        _ => throw new InvalidOperationException()
    };

    public static char ToChar(Pos pos) => 
        pos == Left ? '<' 
        : pos == Right ? '>' 
        : pos == Up ? '^' 
        : pos == Down ? 'v' 
        : throw new InvalidOperationException();

    public static Pos TurnRight(Pos direction) =>
        direction == Up ? Right
        : direction == Right ? Down
        : direction == Down ? Left
        : direction == Left ? Up
        : throw new InvalidOperationException();

    public static Pos TurnLeft(Pos direction) =>
        direction == Up ? Left
        : direction == Left ? Down
        : direction == Down ? Right
        : direction == Right ? Up
        : throw new InvalidOperationException();
}
