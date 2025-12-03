namespace AdventOfCode.Y2025.Day01;

[ProblemName("Secret Entrance")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        int dial = 50;
        int zeroCount = 0;
        foreach (var clicks in lines.Select(line => int.Parse(line[1..]) * (line[0] == 'L' ? -1 : 1)))
        {
            dial = (dial + clicks) % 100;
            if (dial == 0)
            {
                zeroCount++;
            }
        }

        return zeroCount;
    }

    public object PartTwo(string[] lines) 
    {
        int dial = 50;
        int zeroCount = 0;
        foreach (var clicks in lines.Select(line => int.Parse(line[1..]) * (line[0] == 'L' ? -1 : 1)))
        {
            var times = Math.Abs(clicks);
            for (int i = 0; i < times; i++)
            {
                dial = clicks > 0 
                    ? (dial == 99 ? 0 : dial + 1) 
                    : (dial == 0 ? 99 : dial - 1);
                if (dial == 0)
                {
                    zeroCount++;
                }
            }
        }

        return zeroCount;
    }
}
