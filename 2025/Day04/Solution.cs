namespace AdventOfCode.Y2025.Day04;

[ProblemName("Printing Department")]
class Solution : Solver 
{            
    public object PartOne(string[] lines) 
    {
        var grid = lines.ToGrid();
        return grid.AllCells()
            .Count(cell => cell.Value == '@' 
                && Directions.All.Count(dir => grid.ValueAt(cell.Pos + dir) == '@') < 4);
    }

    public object PartTwo(string[] lines) 
    {
        var grid = lines.ToGrid();
        var totalRollsRemoved = 0;
        while (true)
        {
            var rollsToRemove = grid.AllCells()
                .Where(cell => cell.Value == '@' 
                    && Directions.All.Count(dir => grid.ValueAt(cell.Pos + dir) == '@') < 4);
            
            if (!rollsToRemove.Any())
            {
                return totalRollsRemoved;
            }

            foreach (var cell in rollsToRemove)
            {
                grid.SetValueAt(cell.Pos, '.');
                totalRollsRemoved++;
            }
        }
    }
}
