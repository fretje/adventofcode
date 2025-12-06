using System.Diagnostics;

namespace AdventOfCode.Y2025.Day06;

[ProblemName("Trash Compactor")]
partial class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var sum = 0L;
        string[][] input = new string[lines.Length][];
        for (int r = 0; r < lines.Length; r++)
        {
            input[r] = lines[r].Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }
        for (int c = 0; c < input[0].Length; c++)
        {
            sum += input[4][c] switch
            {
                "+" => long.Parse(input[0][c]) + long.Parse(input[1][c]) + long.Parse(input[2][c]) + long.Parse(input[3][c]),
                "*" => long.Parse(input[0][c]) * long.Parse(input[1][c]) * long.Parse(input[2][c]) * long.Parse(input[3][c]),
                _ => throw new UnreachableException(),
            };
        }
        return sum;
    }

    public object PartTwo(string[] lines) 
    {
        var sum = 0L;
        char[] number = new char[4];
        List<long> numbers = [];
        for (int c = lines[0].Length - 1; c >= 0; c--)
        {
            number[0] = lines[0][c];
            number[1] = lines[1][c];
            number[2] = lines[2][c];
            number[3] = lines[3][c];
            if (number[0] is ' ' && number[1] is ' ' && number[2] is ' ' && number[3] is ' ')
            {
                continue;
            }
            numbers.Add(long.Parse(number));
            if (lines[4][c] is '+')
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    sum += numbers[i];
                }
                numbers.Clear();
            }
            else if (lines[4][c] is '*')
            {
                long prod = 1;
                for (int i = 0; i < numbers.Count; i++)
                {
                    prod *= numbers[i];
                }
                sum += prod;
                numbers.Clear();
            }
        }
        return sum;
    }
}
