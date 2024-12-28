namespace AdventOfCode.Y2024.Day19;

[ProblemName("Linen Layout")]
class Solution : Solver 
{            
    public object PartOne(string[] lines)
    {
        var (availables, maxLength, designs) = ParseInput(lines);
        return designs.Count(d => IsPossible(d, availables, maxLength));
    }

    public object PartTwo(string[] lines) 
    {
        var (availables, maxLength, designs) = ParseInput(lines);
        Dictionary<string, long> memo = [];
        return designs.Sum(d => PossibleCount(d, availables, maxLength));
    }

    private static (string[], int, string[]) ParseInput(string[] lines)
    {
        var availables = lines[0].Split(", ");
        return (availables, availables.Max(a => a.Length), lines[2..]);
    }

    private static readonly Dictionary<string, bool> _isPossibleCache = [];
    private static bool IsPossible(string design, string[] availables, int maxLength)
    {
        if (design == "")
        {
            return true;
        }
        if (!_isPossibleCache.TryGetValue(design, out var isPossible))
        {
            for (int i = 0; i <= Math.Min(design.Length, maxLength); i++)
            {
                if (availables.Contains(design[..i])
                    && IsPossible(design[i..], availables, maxLength))
                {
                    isPossible = true;
                    break;
                }
            }
            _isPossibleCache[design] = isPossible;
        }
        return isPossible;
    }

    private static readonly Dictionary<string, long> _possibleCountCache = [];
    private static long PossibleCount(string design, string[] availables, int maxLength)
    {
        if (design == "")
        {
            return 1;
        }
        if (!_possibleCountCache.TryGetValue(design, out var possibleCount))
        {
            for (int i = 0; i <= Math.Min(design.Length, maxLength); i++)
            {
                if (availables.Contains(design[..i]))
                {
                    possibleCount += PossibleCount(design[i..], availables, maxLength);
                }
            }
            _possibleCountCache[design] = possibleCount;
        }
        return possibleCount;
    }
}
