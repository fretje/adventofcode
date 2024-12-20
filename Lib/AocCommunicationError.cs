﻿using System.Text;
namespace AdventOfCode;

public class AocCommuncationError(string title, System.Net.HttpStatusCode? status = null, string details = "") : System.Exception
{
    public readonly string Title = title;
    public readonly System.Net.HttpStatusCode? Status = status;
    public readonly string Details = details;

    public override string Message
    {
        get
        {
            var sb = new StringBuilder();
            sb.AppendLine(Title);
            if (Status != null)
            {
                sb.Append($"[{Status}] ");
            }
            sb.AppendLine(Details);
            return sb.ToString();
        }
    }
}
