using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RepositoriesDownloader
{
    public class Loader
    {
        private const string downloadZipLink = "https://bitbucket.org/{0}/get/{1}.zip"; // https://bitbucket.org/{username}/{repository}/get/default.zip default/master
        private const string gitRepo = "master";
        private const string hgRepo = "default";
        private const string error = "An error occurred during the download: {0}";

        public async Task<string> DownloadZipAsync(string repositoryName, string filePath)
        {
            var link = string.Format(downloadZipLink, repositoryName, gitRepo);
            var attemptCounter = 0;

            while (true)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(link, filePath);
                        return repositoryName;
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        link = string.Format(downloadZipLink, repositoryName, hgRepo);
                    if (attemptCounter++ == 3)
                    {
                        File.Delete(filePath);
                        throw new WebException(string.Format(error, link));
                    }
                    continue;
                }
            }
        }
    }
}
