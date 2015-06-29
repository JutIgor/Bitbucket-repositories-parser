using System.Net;
using System.Threading.Tasks;

namespace RepositoriesDownloader
{
    public class Loader
    {
        private const string downloadZipLink = "https://bitbucket.org/{0}/get/{2}.zip"; // https://bitbucket.org/{username}/{repository}/get/default.zip default/master
        private const string gitRepo = "master";
        private const string hgRepo = "default";
        //private static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //private static string archiveDirectory = appDirectory + @"\..\..\..\Repositories\{0}"; // TODO: create HtmlCss and JS directories
        //private static string archivePath = archiveDirectory + "{0}-{1}.zip";

        //public Loader()
        //{
        //    var directory = string.Format(archiveDirectory, string.Empty); // check
        //    Directory.CreateDirectory(directory);
        //}

        public async Task<string> DownloadZipAsync(string repositoryName, string filePath)
        {
            var link = string.Format(downloadZipLink, repositoryName, gitRepo);
            var attemptCounter = 0;
            //var archiveName = string.Format(archivePath, nickName, repository);

            while (true)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        await client.DownloadFileTaskAsync(link, filePath);
                        return filePath;
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        link = string.Format(downloadZipLink, repositoryName, hgRepo);
                    if (attemptCounter++ == 3)
                        throw new WebException("An error occurred during the download: " + filePath);
                    continue;
                }
            }
        }
    }
}
