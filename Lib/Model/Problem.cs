﻿using AngleSharp.Dom;

namespace AdventOfCode.Model;

internal class Problem
{
    public string Title { get; private set; } = default!;
    public string ContentMd { get; private set; } = default!;
    public int Day { get; private set; }
    public int Year { get; private set; }
    public string Input { get; private set; } = default!;
    public string[] Answers { get; private set; } = default!;

    public static Problem Parse(int year, int day, string url, IDocument document, string input)
    {
        var md = ParseMdIntro(document, 3, url);

        var answers = ParseAnswers(document);
        var title = document.QuerySelector("h2").TextContent;

        var match = Regex.Match(title, ".*: (.*) ---");
        if (match.Success)
        {
            title = match.Groups[1].Value;
        }
        return new() { Year = year, Day = day, Title = title, ContentMd = md, Input = input, Answers = [.. answers] };
    }

    private static string ParseMdIntro(IDocument document, int paragraphs, string url)
    {
        var article = ParseMd(document).Split("\n\n").ToList();
        article = article.Take(Math.Min(paragraphs, article.Count)).ToList();
        article.Add($"Read the [full puzzle]({url}).\n");
        return string.Join("\n\n", article);
    }

    private static string ParseMd(IDocument document)
    {
        var md = "";
        foreach (var article in document.QuerySelectorAll("article"))
        {
            md += UnparseList("", article) + "\n";
        }
        return md;
    }

    private static List<string> ParseAnswers(IDocument document)
    {
        var answers = new List<string>();
        foreach (var article in document.QuerySelectorAll("article"))
        {
            var answerNode = article.NextSibling;
            while (answerNode != null && !(
                answerNode.NodeName == "P"
                && (answerNode as IElement)?.QuerySelector("code") != null
                && answerNode.TextContent.Contains("answer"))
            )
            {
                answerNode = answerNode.NextSibling as IElement;
            }

            var code = (answerNode as IElement)?.QuerySelector("code");
            if (code != null)
            {
                answers.Add(code.TextContent);
            }
        }
        return answers;
    }

    private static string UnparseList(string sep, INode element) => 
        string.Join(sep, element.ChildNodes.SelectMany(Unparse));

    private static IEnumerable<string> Unparse(INode node)
    {
        switch (node.NodeName.ToLower())
        {
            case "h2":
                yield return "## " + UnparseList("", node) + "\n";
                break;
            case "p":
                yield return UnparseList("", node) + "\n";
                break;
            case "em":
                yield return "<em>" + UnparseList("", node) + "</em>";
                break;
            case "code":
                yield return "<code>" + UnparseList("", node) + "</code>";
                break;
            case "span":
                yield return UnparseList("", node);
                break;
            case "s":
                yield return "~~" + UnparseList("", node) + "~~";
                break;
            case "ul":
                foreach (var unparsed in node.ChildNodes.SelectMany(Unparse))
                {
                    yield return unparsed;
                }
                break;
            case "li":
                yield return " - " + UnparseList("", node);
                break;
            case "pre":
                yield return "<pre>\n";
                var freshLine = true;
                foreach (var item in node.ChildNodes)
                {
                    foreach (var unparsed in Unparse(item))
                    {
                        freshLine = unparsed[^1] == '\n';
                        yield return unparsed;
                    }
                }
                yield return freshLine ? "</pre>\n" : "\n</pre>\n";
                break;
            case "a":
                yield return "[" + UnparseList("", node) + "](" + ((IElement)node).Attributes["href"].Value + ")";
                break;
            case "br":
                yield return "\n";
                break;
            case "#text":
                yield return node.TextContent;
                break;
            default:
                throw new NotImplementedException(node.NodeName);
        }
    }
}
