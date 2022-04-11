using System.Linq;
using System.Text.RegularExpressions;

namespace RepoDashboard.Services;

public class LinkHeader
{
    public string FirstLink { get; set; }
    public string PrevLink { get; set; }
    public string NextLink { get; set; }
    public string LastLink { get; set; }

    public static LinkHeader Parse(string headerValue)
    {
        if (string.IsNullOrWhiteSpace(headerValue))
        {
            return null;
        }

        string[] linkStrings = headerValue.Split(',');

        if (!linkStrings.Any())
        {
            return null;
        }

        var linkHeader = new LinkHeader();

        foreach (var linkString in linkStrings)
        {
            var relMatch = Regex.Match(linkString, "(?<=rel=\").+?(?=\")", RegexOptions.IgnoreCase);
            var linkMatch = Regex.Match(linkString, "(?<=<).+?(?=>)", RegexOptions.IgnoreCase);

            if (relMatch.Success && linkMatch.Success)
            {
                var rel = relMatch.Value.ToUpper();
                var link = linkMatch.Value;

                switch (rel)
                {
                    case "FIRST":
                        linkHeader.FirstLink = link;
                        break;
                    case "PREV":
                        linkHeader.PrevLink = link;
                        break;
                    case "NEXT":
                        linkHeader.NextLink = link;
                        break;
                    case "LAST":
                        linkHeader.LastLink = link;
                        break;
                }
            }
        }

        return linkHeader;
    }
}