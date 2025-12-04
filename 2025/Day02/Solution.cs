namespace AdventOfCode.Y2025.Day02;

[ProblemName("Gift Shop")]
class Solution : Solver 
{
    public object PartOne(string[] lines) => Calculate(lines[0]);

    public object PartTwo(string[] lines) => Calculate(lines[0], partTwo: true);

    public static long Calculate(ReadOnlySpan<char> line, bool partTwo = false)
    {
        var sum = 0L;        
        foreach (var range in line.Split(','))
        {
            var rangeSpan = line[range];
            var dashIndex = rangeSpan.IndexOf('-');
            var start = long.Parse(rangeSpan[..dashIndex]);
            var end = long.Parse(rangeSpan[(dashIndex + 1)..]);
            
            for (var num = start; num <= end; num++)
            {
                if (IsInvalid(num, partTwo))
                {
                    sum += num;
                }
            }
        }

        return sum;
    }

    private static bool IsInvalid(long number, bool partTwo)
    {
        var str = number.ToString();
        var len = str.Length;

        if (!partTwo)
        {
            return len % 2 == 0 && str.AsSpan(0, len / 2).SequenceEqual(str.AsSpan(len / 2));
        }

        for (var segmentLength = 1; segmentLength <= len / 2; segmentLength++)
        {
            if (!partTwo && segmentLength != len / 2)
            {
                continue;
            }
            if (len % segmentLength != 0)
            {
                continue;
            }

            var segment = str.AsSpan(0, segmentLength);
            var allMatch = true;
            for (var i = segmentLength; i < len; i += segmentLength)
            {
                if (!str.AsSpan(i, segmentLength).SequenceEqual(segment))
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
            {
                return true;
            }
        }

        return false;
    }
}
