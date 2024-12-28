namespace AdventOfCode.Y2024.Day18;

[ProblemName("RAM Run")]
class Solution : Solver 
{            
    private const int Size = 71;
    private const int FirstBytes = 1024;
    private static readonly Pos Start = new(0, 0);
    private static readonly Pos End = new(Size - 1, Size - 1);

    public object PartOne(string[] lines) => 
        GetGrid(ParseInput(lines)[..FirstBytes]).GetMinimumSteps(Start, End) ?? 0;

    public object PartTwo(string[] lines)
    {
        var bytes = ParseInput(lines);
        var low = FirstBytes;
        var hi = bytes.Length - 1;
        while (low < hi)
        {
            var mid = (low + hi) / 2;
            if (GetGrid(bytes[..(mid + 1)]).GetMinimumSteps(Start, End) is null)
            {
                hi = mid;
            }
            else
            {
                low = mid + 1;
            }
        }
        return $"{bytes[low].Col},{bytes[low].Row}";
    }

    public static Pos[] ParseInput(string[] lines) =>
        [.. lines.Select(line => line.Split(",")).Select(parts => new Pos(int.Parse(parts[0]), int.Parse(parts[1])))];

    public static char[][] GetGrid(Pos[] corruptBytes) => 
        [.. Enumerable.Range(0, Size)
            .Select(row => Enumerable.Range(0, Size)
                .Select(col => corruptBytes.Contains(new(col, row)) ? '#' : '.')
                .ToArray())];
}
