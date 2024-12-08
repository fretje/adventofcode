using System.Net;
using System.Reflection;
using AdventOfCode.Generator;
using AdventOfCode.Model;
using AngleSharp;
using AngleSharp.Io;

namespace AdventOfCode;

internal class Updater
{

    public static async Task Update(int year, int day)
    {
        GitCrypt.Check();

        var session = GetSession();
        var baseAddress = new Uri("https://adventofcode.com/");

        var requester = new DefaultHttpRequester("github.com/encse/adventofcode by encse@csokavar.hu");

        var context = BrowsingContext.New(Configuration.Default
            .With(requester)
            .WithDefaultLoader()
            .WithCss()
            .WithDefaultCookies()
        );
        context.SetCookie(new Url(baseAddress.ToString()), "session=" + session);

        var calendar = await DownloadCalendar(context, baseAddress, year);
        var problem = await DownloadProblem(context, baseAddress, year, day);

        var dir = Dir(year, day);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var years = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(t => t.GetTypeInfo().IsClass && typeof(Solver).IsAssignableFrom(t))
            .Select(SolverExtensions.Year);

        if (!years.Any())
        {
            years = [year];
        }

        UpdateReadmeForYear(calendar);
        UpdateSplashScreen(calendar);
        UpdateReadmeForDay(problem);
        UpdateInput(problem);
        UpdateRefout(problem);
        UpdateSolutionTemplate(problem);
    }

    private static Uri GetBaseAddress() => 
        new("https://adventofcode.com");

    private static string? GetSession() => 
        !Environment.GetEnvironmentVariables().Contains("SESSION")
            ? throw new AocCommuncationError("Specify SESSION environment variable", null)
            : Environment.GetEnvironmentVariable("SESSION");

    private static IBrowsingContext GetContext()
    {
        var context = BrowsingContext.New(Configuration.Default
            .WithDefaultLoader()
            .WithCss()
            .WithDefaultCookies()
        );
        context.SetCookie(new Url(GetBaseAddress().ToString()), "session=" + GetSession());
        return context;
    }

    public static async Task Upload(Solver solver)
    {

        var color = Console.ForegroundColor;
        Console.WriteLine();
        var solverResult = Runner.RunSolver(solver);
        Console.WriteLine();

        if (solverResult.errors.Length != 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Uhh-ohh the solution doesn't pass the tests...");
            Console.ForegroundColor = color;
            Console.WriteLine();
            return;
        }

        var problem = await DownloadProblem(GetContext(), GetBaseAddress(), solver.Year(), solver.Day());

        if (problem.Answers.Length == 2)
        {
            Console.WriteLine("Both parts of this puzzle are complete!");
            Console.WriteLine();
        }
        else if (solverResult.answers.Length <= problem.Answers.Length)
        {
            Console.WriteLine($"You need to work on part {problem.Answers.Length + 1}");
            Console.WriteLine();
        }
        else
        {
            var level = problem.Answers.Length + 1;
            var answer = solverResult.answers[problem.Answers.Length];
            Console.WriteLine($"Uploading answer ({answer}) for part {level}...");

            // https://adventofcode.com/{year}/day/{day}/answer
            // level={part}&answer={answer}

            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler) { BaseAddress = GetBaseAddress() };

            var content = new FormUrlEncodedContent([
                new("level", level.ToString()),
                new("answer", answer),
            ]);

            cookieContainer.Add(GetBaseAddress(), new Cookie("session", GetSession()));
            var result = await client.PostAsync($"/{solver.Year()}/day/{solver.Day()}/answer", content);
            result.EnsureSuccessStatusCode();
            var responseString = await result.Content.ReadAsStringAsync();

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(responseString));
            var article = document.Body.QuerySelector("body > main > article").TextContent;
            article = Regex.Replace(article, @"\[Continue to Part Two.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"You have completed Day.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"\(You guessed.*", "", RegexOptions.Singleline);
            article = Regex.Replace(article, @"  ", "\n", RegexOptions.Singleline);

            if (article.StartsWith("That's the right answer") || article.Contains("You've finished every puzzle"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
                await Update(solver.Year(), solver.Day());
            }
            else if (article.StartsWith("That's not the right answer"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
            }
            else if (article.StartsWith("You gave an answer too recently"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
                Console.WriteLine();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(article);
                Console.ForegroundColor = color;
            }
        }
    }

    private static void WriteFile(string file, string content)
    {
        Console.WriteLine($"Writing {file}");
        File.WriteAllText(file, content);
    }

    private static string Dir(int year, int day) => SolverExtensions.WorkingDir(year, day);

    private static async Task<Calendar> DownloadCalendar(IBrowsingContext context, Uri baseUri, int year)
    {
        var document = await context.OpenAsync(baseUri.ToString() + year);
        return document.StatusCode != HttpStatusCode.OK
            ? throw new AocCommuncationError("Could not fetch calendar", document.StatusCode, document.TextContent)
            : Calendar.Parse(year, document);
    }

    private static async Task<Problem> DownloadProblem(IBrowsingContext context, Uri baseUri, int year, int day)
    {
        var uri = baseUri + $"{year}/day/{day}";
        var color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Updating " + uri);
        Console.ForegroundColor = color;

        var problemStatement = await context.OpenAsync(uri);
        var input = await context.GetService<IDocumentLoader>().FetchAsync(
                new DocumentRequest(new Url(baseUri + $"{year}/day/{day}/input"))).Task;

        return input.StatusCode != HttpStatusCode.OK
            ? throw new AocCommuncationError("Could not fetch input", input.StatusCode, new StreamReader(input.Content).ReadToEnd())
            : Problem.Parse(
                year, day, baseUri + $"{year}/day/{day}", problemStatement,
                new StreamReader(input.Content).ReadToEnd());
    }

    private static void UpdateReadmeForDay(Problem problem) => 
        WriteFile(Path.Combine(Dir(problem.Year, problem.Day), "README.md"), problem.ContentMd);

    private static void UpdateSolutionTemplate(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "Solution.cs");
        if (!File.Exists(file))
        {
            WriteFile(file, SolutionTemplateGenerator.Generate(problem));
        }
    }

    private static void UpdateReadmeForYear(Calendar calendar)
    {
        var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "README.md");
        WriteFile(file, ReadmeGeneratorForYear.Generate(calendar));

        var svg = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "calendar.svg");
        WriteFile(svg, calendar.ToSvg());
    }

    private static void UpdateSplashScreen(Calendar calendar)
    {
        var file = Path.Combine(SolverExtensions.WorkingDir(calendar.Year), "SplashScreen.cs");
        WriteFile(file, SplashScreenGenerator.Generate(calendar));
    }

    private static void UpdateInput(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "input.in");
        WriteFile(file, problem.Input);
    }

    private static void UpdateRefout(Problem problem)
    {
        var file = Path.Combine(Dir(problem.Year, problem.Day), "input.refout");
        if (problem.Answers.Length != 0)
        {
            WriteFile(file, string.Join("\n", problem.Answers));
        }
    }
}
