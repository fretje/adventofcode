namespace AdventOfCode.Y2024.Day24;

[ProblemName("Crossed Wires")]
class Solution : Solver
{
    public object PartOne(string[] lines) => GetOutput(ParseInput(lines));

    public object PartTwo(string[] lines)
    {
        var gates = ParseInput(lines);
        var wires = gates.SelectMany(gates => new[] { gates.Input1, gates.Input2, gates.Output })
            .Distinct().ToDictionary(x => x.Name, x => x);

        List<(string, string)> gateSwaps = [("z08", "ffj"), ("dwp", "kfm"), ("z22", "gjh"), ("z31", "jdr")];
        foreach (var (a, b) in gateSwaps)
        {
            var gate1 = gates.Single(g => g.Output.Name == a);
            var gate2 = gates.Single(g => g.Output.Name == b);
            (gate1.Output, gate2.Output) = (gate2.Output, gate1.Output);
        }

        foreach (var wire in wires.Values)
        {
            wire.In = gates.Where(g => g.Output == wire).SingleOrDefault();
            wire.Outs = [.. gates.Where(g => g.Input1 == wire || g.Input2 == wire)];
        }

        foreach (var bit in Enumerable.Range(0, 45))
        {
            foreach (var wire in wires)
            {
                wire.Value.Value = null;
            }
            var x = 1UL << bit;
            var y = 1UL << bit;
            var expectedZ = x + y;
            SetNumber('x', x, gates);
            SetNumber('y', y, gates);
            var z = GetOutput(gates);
            if (z != expectedZ)
            {
                Console.WriteLine($"Bit {bit}: {x} + {y} = {z}");
                Console.WriteLine($"Expected {expectedZ} but got {z}");
            }
        }
        return string.Join(',', gateSwaps.SelectMany(x => new string[] { x.Item1, x.Item2 }).Order());
    }

    private static List<Gate> ParseInput(string[] lines)
    {
        var readingWires = true;
        List<Wire> wires = [];
        List<Gate> gates = [];
        int gateId = 0;
        foreach (var line in lines)
        {
            if (line == "")
            {
                readingWires = false;
                continue;
            }
            if (readingWires)
            {
                var wireParts = line.Split(": ");
                wires.Add(new(wireParts[0], wireParts[1] == "1"));
                continue;
            }
            var gateParts = line.Split(' ');
            gates.Add(new Gate(gateId, gateParts[1], GetWire(gateParts[0]), GetWire(gateParts[2]), GetWire(gateParts[4])));
            gateId++;
        }

        foreach (var wire in wires)
        {
            wire.In = gates.Where(g => g.Output == wire).SingleOrDefault();
            wire.Outs = [.. gates.Where(g => g.Input1 == wire || g.Input2 == wire)];
        }

        return gates;

        Wire GetWire(string name)
        {
            if (wires.SingleOrDefault(w => w.Name == name) is not { } wire)
            {
                wire = new(name);
                wires.Add(wire);
            }
            return wire;
        }
    }

    private static ulong GetOutput(List<Gate> gates)
    {
        while (gates.Where(g => g.Output.Name.StartsWith('z')).Any(g => g.Output.Value is null))
        {
            foreach (var gate in gates)
            {
                if (gate.Input1.Value.HasValue && gate.Input2.Value.HasValue)
                {
                    gate.Output.Value = gate.Type switch
                    {
                        "AND" => gate.Input1.Value & gate.Input2.Value,
                        "OR" => gate.Input1.Value | gate.Input2.Value,
                        "XOR" => gate.Input1.Value ^ gate.Input2.Value,
                        _ => throw new InvalidOperationException(),
                    };
                }
            }
        }
        return GetNumber('z', gates);
    }

    private static ulong GetNumber(char wirePrefix, List<Gate> gates) =>
        WiresByPrefix(wirePrefix, gates).Aggregate(0UL, (output, wire) => wire.Wire.Value ?? false ? output | (1UL << wire.Index) : output);

    private static void SetNumber(char wirePrefix, ulong value, List<Gate> gates) =>
        WiresByPrefix(wirePrefix, gates).ToList().ForEach(x => x.Wire.Value = (value & (1UL << x.Index)) != 0);

    private static IEnumerable<(Wire Wire, int Index)> WiresByPrefix(char x, List<Gate> gates) => 
        gates.SelectMany(gates => new[] { gates.Input1, gates.Input2, gates.Output })
            .Distinct()
            .Where(w => w.Name.StartsWith(x))
            .OrderBy(w => w.Name)
            .Select((w, i) => (Wire: w, Index: i));

    class Wire(string Name, bool? Value = null)
    {
        public string Name { get; } = Name;
        public bool? Value { get; set; } = Value;
        public Gate? In { get; set; }
        public Gate[] Outs { get; set; } = null!;
        public override string ToString() => $"{Name}: {Value} - From ({In}) to ({string.Join(", ", Outs.Select(x => x.ToString()))})";
    }

    class Gate(int Id, string Type, Wire Input1, Wire Input2, Wire Output)
    {
        public int Id { get; } = Id;
        public string Type { get; } = Type;
        public Wire Input1 { get; } = Input1;
        public Wire Input2 { get; } = Input2;
        public Wire Output { get; set; } = Output;
        public override string ToString() => $"{Id}: {Type}({Input1.Name}, {Input2.Name}) => {Output.Name}";
    }
}
