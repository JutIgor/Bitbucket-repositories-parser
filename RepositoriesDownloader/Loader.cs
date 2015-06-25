using System;
using System.Net;
using System.Threading.Tasks;

namespace RepositoriesDownloader
{
    public class Loader
    {
        private const string downloadZipLink = "https://bitbucket.org/{0}/{1}/get/{2}.zip"; // https://bitbucket.org/{username}/{repository}/get/default.zip default/master
        private const string gitRepo = "master";
        private const string hgRepo = "default";

        private const string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private const string archiveName = @"{0}\repositories\{1}.zip"; // TODO: Check

        public async Task<string> DownloadZipAsync(string nickName, string repository)
        {
            var link = string.Format(downloadZipLink, nickName, repository, gitRepo);
            var attemptCounter = 0;
            string.Format(archiveName, nickName, repository);

            while (true)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(link, archiveName);
                        return archiveName;
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        link = string.Format(downloadZipLink, nickName, repository, hgRepo);
                    if (++attemptCounter == 3)
                        throw new WebException("An error occurred during the download: " + archiveName);
                    continue;
                }
            }
        }
    }
}
