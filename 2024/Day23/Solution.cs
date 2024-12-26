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

    public object PartTwo(string[] lines) => string.Join(",", GetMaxClique(ParseInput(lines)).Order());

    private static Dictionary<string, HashSet<string>> ParseInput(string[] lines)
    {
        Dictionary<string, HashSet<string>> graph = [];
        foreach (var line in lines)
        {
            var parts = line.Split("-");
            var (a, b) = (parts[0], parts[1]);
            if (!graph.TryGetValue(a, out var aConnections))
            {
                aConnections = [];
                graph[a] = aConnections;
            }
            if (!graph.TryGetValue(b, out var bConnections))
            {
                bConnections = [];
                graph[b] = bConnections;
            }
            aConnections.Add(b);
            bConnections.Add(a);
        }
        return graph;
    }

    private static HashSet<string> GetMaxClique(Dictionary<string, HashSet<string>> graph) =>
        BronKerbosch([], [.. graph.Keys], [], graph).OrderByDescending(c => c.Count).First();

    private static HashSet<HashSet<string>> BronKerbosch(HashSet<string> R, HashSet<string> P, HashSet<string> X, Dictionary<string, HashSet<string>> graph)
    {
        HashSet<HashSet<string>> cliques = [];
        if (P.Count == 0 && X.Count == 0)
        {
            cliques.Add(new(R));
        }
        while (P.Count != 0)
        {
            var v = P.First();
            HashSet<string> newR = new(R) { v };
            HashSet<string> newP = new(P.Where(x => graph[v].Contains(x)).ToHashSet());
            HashSet<string> newX = new(X.Where(x => graph[v].Contains(x)).ToHashSet());
            cliques.UnionWith(BronKerbosch(newR, newP, newX, graph));
            P.Remove(v);
            X.Add(v);
        }
        return cliques;
    }
}
