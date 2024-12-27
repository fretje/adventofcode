namespace AdventOfCode;

internal static class NetworkX
{
    public static IEnumerable<HashSet<T>> GetCliques<T>(this Dictionary<T, HashSet<T>> graph)
        where T : notnull =>
        graph.BronKerbosch([], [.. graph.Keys], []);

    public static HashSet<T> GetMaxClique<T>(this Dictionary<T, HashSet<T>> graph) 
        where T: notnull =>
        graph.GetCliques().OrderByDescending(c => c.Count).First();

    private static HashSet<HashSet<T>> BronKerbosch<T>(this Dictionary<T, HashSet<T>> graph, HashSet<T> R, HashSet<T> P, HashSet<T> X)
        where T : notnull
    {
        HashSet<HashSet<T>> cliques = [];
        if (P.Count == 0 && X.Count == 0)
        {
            cliques.Add(new(R));
        }
        while (P.Count != 0)
        {
            var v = P.First();
            HashSet<T> newR = new(R) { v };
            HashSet<T> newP = new(P.Where(x => graph[v].Contains(x)).ToHashSet());
            HashSet<T> newX = new(X.Where(x => graph[v].Contains(x)).ToHashSet());
            cliques.UnionWith(BronKerbosch(graph, newR, newP, newX));
            P.Remove(v);
            X.Add(v);
        }
        return cliques;
    }
}
