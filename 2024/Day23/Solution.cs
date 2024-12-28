namespace AdventOfCode.Y2024.Day23;

[ProblemName("LAN Party")]
class Solution : Solver 
{            
    public object PartOne(string[] lines)
    {
        var graph = ParseInput(lines);
        return graph.Keys
            .SelectMany(a => graph[a].Where(b => b.CompareTo(a) > 0)
                .SelectMany(b => graph[a].Intersect(graph[b])
                    .Where(c => c.CompareTo(a) > 0 && c.CompareTo(b) > 0
                        && (a.StartsWith('t') || b.StartsWith('t') || c.StartsWith('t')))))
            .Count();
    }

    public object PartTwo(string[] lines) => string.Join(",", ParseInput(lines).GetMaxClique().Order());

    private static Dictionary<string, HashSet<string>> ParseInput(string[] lines)
    {
        Dictionary<string, HashSet<string>> graph = [];
        foreach (var line in lines)
        {
            var parts = line.Split("-");
            var (a, b) = (parts[0], parts[1]);
            if (!graph.TryGetValue(a, out var aConnections))
            {
                graph[a] = aConnections = [];
            }
            if (!graph.TryGetValue(b, out var bConnections))
            {
                graph[b] = bConnections = [];
            }
            aConnections.Add(b);
            bConnections.Add(a);
        }
        return graph;
    }
}
