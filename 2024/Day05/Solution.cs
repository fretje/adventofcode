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
        PagesComparer pagesComparer = new(rules);
        return pagesList
            .Where(pages => !IsValid(pages, rules))
            .Sum(pages => MiddleOf([.. pages.Order(pagesComparer)]));
    }

    private static (List<(int, int)>, List<int[]>) Parse(string[] lines)
    {
        (List<(int Left, int Right)> rules, List<int[]> pagesList) = ([], []);
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
            if (rules.Any(r => r.Left == pages[i] && pages[0..i].Contains(r.Right))
                || rules.Any(r => r.Right == pages[i] && pages[(i + 1)..].Contains(r.Right)))
            {
                return false;
            }
        }
        return true;
    }

    private static int MiddleOf(int[] pages) => pages[pages.Length / 2];

    private class PagesComparer(List<(int, int)> rules) : IComparer<int>
    {
        private readonly List<(int Left, int Right)> _rules = rules;

        public int Compare(int x, int y)
        {
            if (_rules.Any(r => r.Left == x && y == r.Right))
            {
                return -1;
            }
            if (_rules.Any(r => r.Left == y && x == r.Right))
            {
                return 1;
            }
            return 0;
        }
    }
}
