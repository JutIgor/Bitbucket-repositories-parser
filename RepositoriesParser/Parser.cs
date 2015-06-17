using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RepositoriesParser
{
    public class Parser
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private const string nickPattern = @"<a class=""avatar-link"" href=""/";
        private const string nextPagePattern = @"<a href=""/repo/all/2?name=[a-z]{2}&language=html%2Fcss""";
        private string result;

        private IEnumerable<string> SearchQuery()
        {
            return alphabet.SelectMany(x => alphabet.Select(y => x.ToString() + y.ToString()));
        }

        private int SearchEndNick(int startPosition)
        {
            while(true)
            {
                if (result[startPosition++] == '/') return startPosition - 2;
            }
        }

        public IEnumerable<string> GetUsers(string searchParameter)
        {
            using (var client = new WebClient())
            {
                foreach (var item in SearchQuery())
                {
                    var path = "https://bitbucket.org/repo/all?name=" + item + "&language=html%2Fcss";
                    result = client.DownloadString(path);

                    var matches = Regex.Matches(result, nickPattern);
                    foreach (Match match in matches)
                    {
                        
                    }
                }
            }
        }
    }
}
