namespace AdventOfCode.Y2024.Day17;

[ProblemName("Chronospatial Computer")]
class Solution : Solver 
{
    public object PartOne(string[] lines)
    {
        var (regA, regB, regC, program) = ParseInput(lines);
        return string.Join(',', ExecuteProgram(regA, regB, regC, program));
    }

    public object PartTwo(string[] lines)
    {
        var (_, _, _, program) = ParseInput(lines);
        return FindCorrectRegA(program, 0) ?? throw new InvalidOperationException("No correct reg a was found");
    }

    private static (long, long, long, int[]) ParseInput(string[] lines)
    {
        var regA = long.Parse(lines[0].Split(' ').Last());
        var regB = long.Parse(lines[1].Split(' ').Last());
        var regC = long.Parse(lines[2].Split(' ').Last());
        var program = lines[4].Split(' ')[1].Split(',').Select(int.Parse).ToArray();
        return (regA, regB, regC, program);
    }

    private static List<int> ExecuteProgram(long regA, long regB, long regC, int[] program)
    {
        List<int> output = [];
        var instructionPtr = 0;
        while (instructionPtr < program.Length)
        {
            var instruction = program[instructionPtr];
            var operand = program[instructionPtr + 1];
            instructionPtr += 2;
            if (instruction == 0) // adv
            {
                regA >>= (int)Combo(operand);
            }
            else if (instruction == 1) // bxl
            {
                regB ^= operand;
            }
            else if (instruction == 2) // bst
            {
                regB = Combo(operand) % 8;
            }
            else if (instruction == 3) // jnz
            {
                if (regA != 0)
                {
                    instructionPtr = operand;
                }
            }
            else if (instruction == 4) // bxc
            {
                regB ^= regC;
            }
            else if (instruction == 5) // out
            {
                output.Add((int)(Combo(operand) % 8));
            }
            else if (instruction == 6) // bdv
            {
                regB = regA >> (int)Combo(operand);
            }
            else if (instruction == 7) // cdv
            {
                regC = regA >> (int)Combo(operand);
            }
        }
        return output;

        long Combo(int operand) => operand switch
        {
            >= 0 and <= 3 => operand,
            4 => regA,
            5 => regB,
            6 => regC,
            _ => throw new Exception("Invalid combo")
        };
    }

    private static long? FindCorrectRegA(int[] program, long regA)
    {
        if (program.Length == 0)
        {
            return regA;
        }
        foreach (var i in Enumerable.Range(0, 8))
        {
            long b = i;
            long a = regA << 3 | b;
            b ^= 1;
            long c = a >> (int)b;
            b ^= c;
            b ^= 6;
            if (b % 8 == program[^1])
            {
                if (FindCorrectRegA(program[..^1], a) is { } correctRegA)
                {
                    return correctRegA;
                }
            }
        }
        return null;
    }
}
