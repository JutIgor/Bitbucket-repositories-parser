using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RepositoriesParser
{
    public class Parser
    {
        private string source = string.Empty;
        private Match nextPage;
        private int currentPage = 1;
        public int requestCounter = 0;

        private IEnumerable<string> SearchQuery()
        {
            var alphabet = "abcdefghijklmnopqrstuvwxyz";
            return alphabet.SelectMany(x => alphabet.Select(y => x.ToString() + y.ToString()));
        }

        private void GetSource(string searchParameter)
        {
            var link = string.Format(Links.bitbucketLink, searchParameter);
            using (var client = new WebClient())
            {
                source = client.DownloadString(link);
                requestCounter++;
            }
        }

        public IEnumerable<string> GetUser(string language)
        {
            foreach (var item in SearchQuery())
            {
                var path = string.Format(Links.searchLink, item, language);
                currentPage = 1;
                do
                {
                    var nextPageLink = string.Format(Patterns.nextSearchPagePattern, ++currentPage, language);
                    nextPage = Regex.Match(source, nextPageLink);
                    if (nextPage.Success)
                        GetSource(nextPage.Value);
                    else
                        GetSource(path);

                    var matches = Regex.Matches(source, Patterns.nickPattern);
                    foreach (Match match in matches)
                    {
                        yield return match.Groups["nickName"].Value;
                    }
                } while (nextPage.Success);
            }
        }

        public IEnumerable<string> GetRepository(string userName, string language)
        {
            var path = string.Format(Links.userProfileLink, userName, language);
            GetSource(path);
            do
            {
                var nextPageLink = string.Format(Patterns.nextProfilePagePattern, userName, ++currentPage);
                nextPage = Regex.Match(source, nextPageLink);

                var matches = Regex.Matches(source, Patterns.repositoryPattern);
                foreach (Match match in matches)
                {
                    yield return match.Groups["repoName"].Value;
                }
                if (nextPage.Success)
                    GetSource(nextPage.Value);
            } while (nextPage.Success);
        }
    }
}
