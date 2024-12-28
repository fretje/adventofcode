using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode;

[AttributeUsage(AttributeTargets.Class)]
internal class ProblemName(string name) : Attribute
{
    public readonly string Name = name;
}

internal interface Solver
{
    object PartOne(string[] lines);
    object? PartTwo(string[] lines) => null;
}

internal static class SolverExtensions
{
    public static IEnumerable<object> Solve(this Solver solver, string[] lines)
    {
        yield return solver.PartOne(lines);
        var res = solver.PartTwo(lines);
        if (res != null)
        {
            yield return res;
        }
    }

    public static string? GetName(this Solver solver) =>
        solver.GetType().GetCustomAttribute<ProblemName>()?.Name;

    public static string DayName(this Solver solver) => $"Day {solver.Day()}";

    public static int Year(this Solver solver) => Year(solver.GetType());

    public static int Year(Type t) => int.Parse(t.FullName!.Split('.')[1][1..]);
    public static int Day(this Solver solver) => Day(solver.GetType());

    public static int Day(Type t) => int.Parse(t.FullName!.Split('.')[2][3..]);

    public static string WorkingDir(int year) => Path.Combine(year.ToString());

    public static string WorkingDir(int year, int day) => Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));

    public static string WorkingDir(this Solver solver) => WorkingDir(solver.Year(), solver.Day());

    public static SplashScreen SplashScreen(this Solver solver) =>
        (SplashScreen)Activator.CreateInstance(
            Assembly.GetEntryAssembly()!.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass && typeof(SplashScreen).IsAssignableFrom(t))
                .Single(t => Year(t) == solver.Year()))!;

    public static int Sloc(this Solver solver)
    {
        var file = solver.WorkingDir() + "/Solution.cs";
        if (File.Exists(file))
        {
            var solution = File.ReadAllText(file);
            return Regex.Matches(solution, @"\n").Count;
        }
        return -1;
    }
}

internal record SolverResult(string[] answers, string[] errors);

internal class Runner
{
    private static string[] GetInput(string file)
    {
        GitCrypt.CheckFile(file);
        return File.ReadAllLines(file);
    }

    public static SolverResult RunSolver(Solver solver)
    {
        var workingDir = solver.WorkingDir();
        var indent = "    ";
        Write(ConsoleColor.White, $"{indent}{solver.DayName()}: {solver.GetName()}");
        WriteLine();
        var file = Path.Combine(workingDir, "input.in");
        var refoutFile = file.Replace(".in", ".refout");
        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
        var input = GetInput(file);
        var iline = 0;
        var answers = new List<string>();
        var errors = new List<string>();
        var timestamp = Stopwatch.GetTimestamp();

        foreach (var line in solver.Solve(input))
        {
            var elapsedTime = Stopwatch.GetElapsedTime(timestamp);
            if (line is OcrString ocrString)
            {
                Console.WriteLine("\n" + ocrString.st.Indent(10, firstLine: true));
            }
            answers.Add(line.ToString()!);
            var (statusColor, status, err) =
                refout == null || refout.Length <= iline ? (ConsoleColor.Cyan, "?", null) :
                refout[iline] == line.ToString() ? (ConsoleColor.DarkGreen, "✓", null) :
                (ConsoleColor.Red, "X", $"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");

            if (err != null)
            {
                errors.Add(err);
            }

            Write(statusColor, $"{indent}  {status}");
            Console.Write($" {line} ");
            var diff = elapsedTime.TotalMilliseconds;

            WriteLine(
                diff > 1000 ? ConsoleColor.Red :
                diff > 500 ? ConsoleColor.Yellow :
                ConsoleColor.DarkGreen,
                $"({diff:F3} ms)"
            );
            iline++;
            timestamp = Stopwatch.GetTimestamp();
        }

        return new([.. answers], [.. errors]);
    }

    public static void RunAll(params Solver[] solvers)
    {
        if (solvers.Length == 0)
        {
            WriteLine(ConsoleColor.Yellow, "No solvers found. To start writing one, use");
            WriteLine(ConsoleColor.Yellow, "> dotnet run update 20xx/xx");
            WriteLine();
            return;
        }

        var errors = new List<string>();

        var lastYear = -1;
        List<(int day, int sloc)> slocs = [];
        foreach (var solver in solvers)
        {
            if (lastYear != solver.Year())
            {
                SlocChart.Show(slocs);
                slocs.Clear();

                solver.SplashScreen().Show();
                lastYear = solver.Year();
            }
            slocs.Add((solver.Day(), solver.Sloc()));
            var result = RunSolver(solver);
            WriteLine();
            errors.AddRange(result.errors);
        }
        SlocChart.Show(slocs);
        WriteLine();

        if (errors.Count != 0)
        {
            WriteLine(ConsoleColor.Red, "Errors:\n" + string.Join("\n", errors));
        }

        WriteLine(ConsoleColor.Yellow, "Please support the maintainer: https://github.com/sponsors/encse");
        WriteLine();
    }

    private static void WriteLine(ConsoleColor color = ConsoleColor.Gray, string text = "") => 
        Write(color, text + "\n");

    private static void Write(ConsoleColor color = ConsoleColor.Gray, string text = "")
    {
        var c = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ForegroundColor = c;
    }
}
