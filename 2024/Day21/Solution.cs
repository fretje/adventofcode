namespace AdventOfCode.Y2024.Day21;

[ProblemName("Keypad Conundrum")]
class Solution : Solver 
{
    private static readonly char[][] _numpad = [
        ['7', '8', '9'],
        ['4', '5', '6'],
        ['1', '2', '3'],
        ['#', '0', 'A'],
    ];
    private static readonly char[][] _dirpad = [
        ['#', '^', 'A'],
        ['<', 'v', '>'],
    ];

    public object PartOne(string[] lines) => Solve(lines);

    public object PartTwo(string[] lines) => Solve(lines, 25);

    private static long Solve(string[] lines, int numberOfDirpads = 2) =>
        lines.Sum(line => 
            GetPossibleSequences(line, NumpadSequences)
                .Min(long (input) => ('A' + input).Zip(input, (from, to) => GetMinDirpadLength(from, to, numberOfDirpads)).Sum())
            * int.Parse(line[..^1]));

    private static string[] GetPossibleSequences(string input, Dictionary<(char, char), List<string>> padSequences) =>
        [.. ('A' + input).Zip(input, (x, y) => padSequences[(x, y)]).CartesianProduct().Select(x => string.Concat(x))];

    private static Dictionary<(char, char, int), long>? _minDirpadLengthCache;
    private static long GetMinDirpadLength(char from, char to, int depth = 2)
    {
        _minDirpadLengthCache ??= DirpadSequences.ToDictionary(x => (x.Key.From, x.Key.To, 1), x => (long)x.Value.First().Length); // init the cache's depth 1
        if (!_minDirpadLengthCache.TryGetValue((from, to, depth), out var result))
        {
            result = DirpadSequences[(from, to)].Min(seq => ('A' + seq).Zip(seq, (from, to) => GetMinDirpadLength(from, to, depth - 1)).Sum());
            _minDirpadLengthCache[(from, to, depth)] = result;
        }
        return result;
    }

    private static Dictionary<(char, char), List<string>> NumpadSequences { get; } = GetPadSequences(_numpad);
    private static Dictionary<(char From, char To), List<string>> DirpadSequences { get; } = GetPadSequences(_dirpad);

    private static Dictionary<(char, char), List<string>> GetPadSequences(char[][] pad)
    {
        var padPositions = pad.AllCells().Where(c => c.Value != '#').ToDictionary(x => x.Value, x => x.Pos);
        return padPositions.Keys.SelectMany(from => padPositions.Keys.Select(to => (from, to)))
            .ToDictionary(x => x, x => x.from == x.to ? ["A"] : GetPadSequences(padPositions[x.from], padPositions[x.to], pad));
    }

    private static List<string> GetPadSequences(Pos from, Pos to, char[][] pad)
    {
        List<string> sequences = [];
        Queue<(Pos, string)> queue = [];
        queue.Enqueue((from, ""));
        var minLength = int.MaxValue;
        while (queue.Count != 0)
        {
            var (pos, seq) = queue.Dequeue();
            foreach (var (nextPos, nextMove) in (List<(Pos, char)>)
                [(pos + Directions.Up, '^'), (pos + Directions.Down, 'v'), (pos + Directions.Left, '<'), (pos + Directions.Right, '>')])
            {
                if (!pad.Contains(nextPos) || pad.ValueAt(nextPos) == '#')
                {
                    continue;
                }
                if (nextPos == to)
                {
                    if (minLength < seq.Length + 1)
                    {
                        return sequences;
                    }
                    minLength = seq.Length + 1;
                    sequences.Add(seq + nextMove + 'A');
                    continue;
                }
                queue.Enqueue((nextPos, seq + nextMove));
            }
        }
        return sequences;
    }
}
