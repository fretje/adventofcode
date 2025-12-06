namespace AdventOfCode.Y2025.Day06;

[ProblemName("Trash Compactor")]
partial class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var input = new string[lines.Length][];
        for (int row = 0; row < lines.Length; row++)
        {
            input[row] = lines[row].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
        var total = 0L;
        for (var col = 0; col < input[0].Length; col++)
        {
            if (input[^1][col] is "+")
            {
                for (var row = 0; row < input.Length - 1; row++)
                {
                    total += long.Parse(input[row][col]);
                }
            }
            else if (input[^1][col] is "*")
            {
                var prod = 1L;
                for (var row = 0; row < input.Length - 1; row++)
                {
                    prod *= long.Parse(input[row][col]);
                }
                total += prod;
            }
        }
        return total;
    }

    public object PartTwo(string[] lines) 
    {
        var total = 0L;
        var number = new char[lines.Length - 1];
        List<long> numbers = [];
        for (var col = lines[0].Length - 1; col >= 0; col--)
        {
            var isAllSpaces = true;
            for (var row = 0; row < lines.Length - 1; row++)
            {
                number[row] = lines[row][col];
                if (lines[row][col] is ' ')
                {
                    continue;
                }
                isAllSpaces = false;
            }
            if (isAllSpaces)
            {
                continue;
            }
            numbers.Add(long.Parse(number));
            if (lines[^1][col] is '+')
            {
                for (var i = 0; i < numbers.Count; i++)
                {
                    total += numbers[i];
                }
                numbers.Clear();
            }
            else if (lines[^1][col] is '*')
            {
                var prod = 1L;
                for (var i = 0; i < numbers.Count; i++)
                {
                    prod *= numbers[i];
                }
                total += prod;
                numbers.Clear();
            }
        }
        return total;
    }
}
