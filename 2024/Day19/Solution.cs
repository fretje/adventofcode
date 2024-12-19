namespace AdventOfCode.Y2024.Day19;

[ProblemName("Linen Layout")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var availables = lines[0].Split(", ");
        var maxLength = availables.Max(a => a.Length);
        var designs = lines[2..];
        return designs.Count(d => IsPossible(d, availables, maxLength));
    }

    public object PartTwo(string[] lines) 
    {
        var availables = lines[0].Split(", ");
        var maxLength = availables.Max(a => a.Length);
        var designs = lines[2..];
        return designs.Sum(d => PossibleCount(d, availables, maxLength));
    }

    private static bool IsPossible(string design, string[] availables, int maxLength, Dictionary<string, bool>? memo = null)
    {
        if (design == "")
        {
            return true;
        }
        memo ??= [];
        if (memo.TryGetValue(design, out var isPossible))
        {
            return isPossible;
        }
        for (int i = 0; i <= Math.Min(design.Length, maxLength); i++)
        {
            if (availables.Contains(design[..i]) 
                && IsPossible(design[i..], availables, maxLength, memo))
            {
                isPossible = true;
                break;
            }
        }
        memo[design] = isPossible;
        return isPossible;
    }

    private static long PossibleCount(string design, string[] availables, int maxLength, Dictionary<string, long>? memo = null)
    {
        if (design == "")
        {
            return 1;
        }
        memo ??= [];
        if (memo.TryGetValue(design, out var possibleCount))
        {
            return possibleCount;
        }
        for (int i = 0; i <= Math.Min(design.Length, maxLength); i++)
        {
            if (availables.Contains(design[..i]))
            {
                possibleCount += PossibleCount(design[i..], availables, maxLength, memo);
            }
        }
        memo[design] = possibleCount;
        return possibleCount;
    }
}
