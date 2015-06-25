namespace RepositoriesParser
{
    public class Patterns
    {
        public const string nickPattern = @"<a class=""avatar-link"" href=""/(?<nickName>[^/]+)";
        public const string nextSearchPagePattern = @"/repo/all/{0}\?name=[a-z]+&language={1}"; // language=html%2Fcss/language=javascript
        public const string repositoryPattern = @"<a class=""repo-avatar-link"" href=""/(?<nickName>[^/]+)/(?<repoName>[^""]+)"">";
        public const string nextProfilePagePattern = @"/{0}\?page={1}";

        public const string gitRepo = "master";
        public const string hgRepo = "default";
    }
}
