using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RepositoriesParser
{
    public class Parser
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string nickPattern = @"<a class=""avatar-link"" href=""/(?<nickName>[^/]+)";
        private const string nextPagePattern = @"/repo/all/{0}\?name=[a-z]+&language=javascript"; // language=html%2Fcss
        private const string bitBucket = "https://bitbucket.org{0}";
        private string searchLink = "/repo/all?name={0}&language=html%2Fcss";
        private string source = string.Empty;
        public int requestCounter = 0;
        private IEnumerable<string> SearchQuery()
        {
            return alphabet.SelectMany(x => alphabet.Select(y => x.ToString() + y.ToString()));
        }

        private void GetSource(string searchParameter)
        {
            var link = string.Format(bitBucket, searchParameter);
            using (var client = new WebClient())
            {
                source = client.DownloadString(link);
                requestCounter++;
            }
        }

        public IEnumerable<string> GetUser()
        {
            var currentPage = 1;
            Match nextPage;
            foreach (var item in SearchQuery())
            {
                var path = string.Format(searchLink, item);
                currentPage = 1;
                do
                {
                    var nextPageLink = string.Format(nextPagePattern, ++currentPage);
                    nextPage = Regex.Match(source, nextPageLink);
                    if (nextPage.Success)
                        GetSource(nextPage.Value);
                    else
                        GetSource(path);

                    var matches = Regex.Matches(source, nickPattern);
                    foreach (Match match in matches)
                    {
                        yield return match.Groups["nickName"].Value;
                    }
                } while (nextPage.Success);
            }
        }
    }
}
