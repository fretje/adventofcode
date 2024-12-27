namespace AdventOfCode.Y2024.Day05;

[ProblemName("Print Queue")]
internal class Solution : Solver
{
    public object PartOne(string[] lines)
    {
        var (rules, pagesList) = Parse(lines);
        return pagesList.Where(p => IsValid(p, rules)).Sum(MiddleOf);
    }

    public object PartTwo(string[] lines)
    {
        var (rules, pagesList) = Parse(lines);
        return pagesList
            .Where(pages => !IsValid(pages, rules))
            .Sum(pages => MiddleOf(Sort(pages, rules)));
    }

    private static (List<(int Left, int Right)> Rules, List<int[]> PagesList) Parse(string[] lines)
    {
        List<(int Left, int Right)> rules = [];
        List<int[]> pagesList = [];
        bool readingRules = true;
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                readingRules = false;
                continue;
            }
            if (readingRules)
            {
                var parts = line.Split("|");
                rules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
                continue;
            }
            pagesList.Add(line.Split(',').Select(int.Parse).ToArray());
        }
        return (rules, pagesList);
    }

    private static bool IsValid(int[] pages, List<(int Left, int Right)> rules)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if ((i > 0 && rules.Any(r => r.Left == pages[i] && pages[0..i].Contains(r.Right)))
                || (i < pages.Length - 1 && rules.Any(r => r.Right == pages[i] && pages[(i + 1)..].Contains(r.Right))))
            {
                return false;
            }
        }
        return true;
    }

    private static int MiddleOf(int[] pages) => pages[pages.Length / 2];

    private static int[] Sort(int[] pages, List<(int Left, int Right)> rules)
    {
        while (!IsValid(pages, rules))
        {
            for (int i = 0; i < pages.Length; i++)
            {
                if (i > 0 && rules.FirstOrDefault(r => r.Left == pages[i] && pages[0..i].Contains(r.Right)) is { Left: > 0, Right: > 0 } rule)
                {
                    var index = Array.IndexOf(pages, rule.Right);
                    (pages[index], pages[i]) = (pages[i], pages[index]);
                }
                else if (i < pages.Length - 1 && rules.FirstOrDefault(r => r.Right == pages[i] && pages[(i + 1)..].Contains(r.Right)) is { Left: > 0, Right: > 0 } rule2)
                {
                    var index = Array.IndexOf(pages, rule2.Right);
                    (pages[index], pages[i]) = (pages[i], pages[index]);
                }
            }
        }
        return pages;
    }
}
