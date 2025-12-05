namespace AdventOfCode.Y2025.Day05;

[ProblemName("Cafeteria")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        long count = 0;
        bool readingRanges = true;
        List<(long Start, long End)> ranges = [];
        foreach (var line in lines)
        {
            if (line is { Length: 0 })
            {
                readingRanges = false;
                continue;
            }

            if (readingRanges)
            {
                var parts = line.Split('-');
                ranges.Add((Start: long.Parse(parts[0]), End: long.Parse(parts[1])));
                continue;
            }

            long number = long.Parse(line);
            foreach (var (start, end) in ranges)
            {
                if (number >= start && number <= end)
                {
                    count++;
                    break;
                }
            }
        }
        return count;
    }

    public object PartTwo(string[] lines) 
    {
        List<FreshRange> ranges = [];
        foreach (var line in lines)
        {
            if (line is { Length: 0 })
            {
                break;
            }

            var parts = line.Split('-');
            ranges.Add(new(long.Parse(parts[0]), long.Parse(parts[1])));
        }

        List<FreshRange> merged = [];
        foreach (var range in ranges.OrderBy(r => r.Start))
        {
            var last = merged.LastOrDefault();
            if (last is null || last.End < range.Start)
            {
                merged.Add(range);
            }
            else
            {
                last.End = Math.Max(last.End, range.End);
            }
        }

        return merged.Sum(r => r.End - r.Start + 1);
    }

    private class FreshRange(long start, long end)
    {
        public long Start { get; set; } = start;
        public long End { get; set; } = end;
    }
}
