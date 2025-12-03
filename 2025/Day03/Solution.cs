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
            int[] maxValue = new int[numberOfBatteries];
            int[] maxIndex = new int[numberOfBatteries];
            var joltage = 0L;

            for (int batteryIndex = 0; batteryIndex < numberOfBatteries; batteryIndex++)
            {
                for (int bankIndex = batteryIndex == 0 ? 0 : maxIndex[batteryIndex-1] + 1; 
                    bankIndex < bank.Length - numberOfBatteries + batteryIndex + 1; 
                    bankIndex++)
                {
                    if (bank[bankIndex] <= maxValue[batteryIndex])
                    {
                        continue;
                    }
                    maxValue[batteryIndex] = bank[bankIndex];
                    maxIndex[batteryIndex] = bankIndex;
                }

                joltage += maxValue[batteryIndex] * (long)Math.Pow(10, numberOfBatteries - batteryIndex - 1);
            }

            return joltage;
        });
}
