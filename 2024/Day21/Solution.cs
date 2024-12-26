namespace AdventOfCode.Y2024.Day21;

[ProblemName("Keypad Conundrum")]
class Solution : Solver 
{
    private static readonly char[][] Numpad = [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        ['#', '0', 'A'],
    ];
    private static Dictionary<char, Pos> NumpadPositions { get; } =
        Numpad.AllCells().Where(c => c.Value != '#').ToDictionary(x => x.Value, x => x.Pos);
    private static Dictionary<(char, char), List<string>> NumpadSequences { get; } =
        GetPadSequences(Numpad, NumpadPositions);

    private static readonly char[][] Dirpad = [
        ['#', '^', 'A'],
        ['<', 'v', '>'],
    ];
    private static Dictionary<char, Pos> DirpadPositions { get; } =
        Dirpad.AllCells().Where(c => c.Value != '#').ToDictionary(x => x.Value, x => x.Pos);
    private static Dictionary<(char, char), List<string>> DirpadSequences { get; } =
        GetPadSequences(Dirpad, DirpadPositions);
    private static Dictionary<(char, char), int> DirpadLengths { get; } =
        DirpadSequences.ToDictionary(x => x.Key, x => x.Value.First().Length);

    public object PartOne(string[] lines) => Solve(lines);

    public object PartTwo(string[] lines) => Solve(lines, 25);

    private static long Solve(string[] lines, int numberOfDirpads = 2) =>
        lines.Sum(line => 
            GetPossibleSequences(line, NumpadSequences)
                .Min(long (input) => ('A' + input).Zip(input, (from, to) => GetLength(from, to, numberOfDirpads)).Sum())
            * int.Parse(line[..^1]));

    private static long GetLength(char from, char to, int depth = 2, Dictionary<(char, char, int), long>? memo = null)
    {
        if (!(memo ??= []).ContainsKey((from, to, depth)))
        {
            memo[(from, to, depth)] = depth == 1
                ? DirpadLengths[(from, to)]
                : DirpadSequences[(from, to)].Min(seq => ('A' + seq).Zip(seq, (from2, to2) => GetLength(from2, to2, depth - 1, memo)).Sum());
        }
        return memo[(from, to, depth)];
    }

    private static string[] GetPossibleSequences(string input, Dictionary<(char, char), List<string>> padSequences) =>
        [.. ('A' + input).Zip(input, (x, y) => padSequences[(x, y)]).CartesianProduct().Select(x => string.Concat(x))];

    private static Dictionary<(char, char), List<string>> GetPadSequences(char[][] pad, Dictionary<char, Pos> padPositions) => 
        padPositions.Keys.SelectMany(from => padPositions.Keys.Select(to => (from, to)))
            .ToDictionary(x => x, x => x.from == x.to ? (["A"]) : GetPadSequences(x.from, x.to, pad, padPositions));

    private static List<string> GetPadSequences(char from, char to, char[][] pad, Dictionary<char, Pos> padPositions)
    {
        List<string> sequences = [];
        Queue<(Pos, string)> queue = [];
        queue.Enqueue((padPositions[from], ""));
        var minLength = int.MaxValue;
        while (queue.Count != 0)
        {
            var (pos, seq) = queue.Dequeue();
            foreach (var (newPos, newMove) in (List<(Pos, char)>)
                [(pos + Directions.Up, '^'), (pos + Directions.Down, 'v'), (pos + Directions.Left, '<'), (pos + Directions.Right, '>')])
            {
                if (!pad.Contains(newPos) || pad.ValueAt(newPos) == '#')
                {
                    continue;
                }
                if (pad.ValueAt(newPos) == to)
                {
                    if (minLength < seq.Length + 1)
                    {
                        return sequences;
                    }
                    minLength = seq.Length + 1;
                    sequences.Add(seq + newMove + 'A');
                }
                else
                {
                    queue.Enqueue((newPos, seq + newMove));
                }
            }
        }
        return sequences;
    }
}
