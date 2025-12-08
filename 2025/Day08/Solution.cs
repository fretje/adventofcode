namespace AdventOfCode.Y2025.Day08;

[ProblemName("Playground")]
class Solution : Solver 
{            
    public object PartOne(string[] lines)
    {
        var (circuits, distances) = ParseInput(lines);

        foreach (var (posA, posB, _) in distances.OrderBy(kv => kv.Distance).Take(1000))
        {
            var circuitA = circuits.First(c => c.Contains(posA));
            var circuitB = circuits.First(c => c.Contains(posB));
            if (circuitA != circuitB)
            {
                circuitA.UnionWith(circuitB);
                circuits.Remove(circuitB);
            }
        }

        return circuits
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Aggregate(1, (prod, circuit) => prod * circuit.Count);
    }

    public object PartTwo(string[] lines) 
    {
        var (circuits, distances) = ParseInput(lines);

        foreach (var (posA, posB, _) in distances.OrderBy(kv => kv.Distance))
        {
            var circuitA = circuits.First(c => c.Contains(posA));
            var circuitB = circuits.First(c => c.Contains(posB));
            if (circuitA != circuitB)
            {
                circuitA.UnionWith(circuitB);
                circuits.Remove(circuitB);
            }
            if (circuits.Count == 1)
            {
                return (long)posA.X * posB.X;
            }
        }

        throw new InvalidOperationException();
    }

    private static (List<HashSet<Pos3D>> Circuits, List<(Pos3D PosA, Pos3D PosB, double Distance)> Distances) ParseInput(string[] lines)
    {
        Pos3D[] nodes = [.. lines
            .Select(line => line.Split(','))
            .Select(parts => new Pos3D(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2])))];
        List<HashSet<Pos3D>> circuits = [.. nodes.Select(node => (HashSet<Pos3D>)[node])];
        List<(Pos3D, Pos3D, double)>  distances = [];
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            for (int j = i + 1; j < nodes.Length; j++)
            {
                var distance = nodes[i].DistanceTo(nodes[j]);
                distances.Add((nodes[i], nodes[j], distance));
            }
        }
        return (circuits, distances);
    }
}
