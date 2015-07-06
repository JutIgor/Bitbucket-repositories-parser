using System.Text.RegularExpressions;

namespace DownloadService
{
    public static class Patterns
    {
        private const string repositoryPattern = @"(HtmlCss|JavaScript)\\(?<repoName>[^.]+)";
        public const string HtmlCss = "HtmlCss";
        public const string JavaScript = "JavaScript";

        public static string GetRepositoryName(string source)
        {
            return Regex.Match(source, repositoryPattern).Groups["repoName"].Value.Replace('-', '/');
        }
    }
}
