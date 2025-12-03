namespace AdventOfCode.Y2025.Day03;

[ProblemName("Lobby")]
class Solution : Solver 
{
    public object PartOne(string[] lines) => Calculate(lines, 2);

    public object PartTwo(string[] lines) => Calculate(lines, 12);

    private static long Calculate(string[] lines, int numberOfBatteries) =>
        lines.Sum(line =>
        {
            var bank = line.Select(x => x - '0').ToArray();
            int bankStartIndex = 0;
            var joltage = 0L;

            for (int batteryIndex = 0; batteryIndex < numberOfBatteries; batteryIndex++)
            {
                int maxValue = 0;
                for (int bankIndex = bankStartIndex; 
                    bankIndex < bank.Length - numberOfBatteries + batteryIndex + 1; 
                    bankIndex++)
                {
                    if (bank[bankIndex] <= maxValue)
                    {
                        continue;
                    }

                    maxValue = bank[bankIndex];
                    bankStartIndex = bankIndex + 1;
                }

                joltage += maxValue * (long)Math.Pow(10, numberOfBatteries - batteryIndex - 1);
            }

            return joltage;
        });
}
