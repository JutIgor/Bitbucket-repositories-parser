using System.Text.RegularExpressions;

namespace DownloadService
{
    public static class Patterns
    {
        private const string repositoryPathPattern = @"(HtmlCss|JavaScript)\\(?<repoName>[^.]+)";
        private const string repositoryLinkPattern = @"org/(?<repoName>[^/]+/[^/]+)";
        public const string HtmlCss = "HtmlCss";
        public const string JavaScript = "JavaScript";

        public static string GetRepositoryName(string source)
        {
            return Regex.Match(source, repositoryLinkPattern).Groups["repoName"].Value;
        }
    }
}
