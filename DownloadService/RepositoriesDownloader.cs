using RepositoriesDownloader;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace DownloadService
{
    [DataContract(Name = "RepositoriesDownloader")]
    public class RepositoriesDownloader
    {
        [DataMember]
        private List<string> finished = new List<string>();
        [IgnoreDataMember]
        private List<Task<string>> downloads = new List<Task<string>>();
        [IgnoreDataMember]
        private Loader loader = new Loader();
        [IgnoreDataMember]
        private Task<string> finishedTask;
        [IgnoreDataMember]
        private bool isStopped;
        [DataMember]
        private bool isFinished;

        public void AllocateMemoryForDownloads()
        {
            this.downloads = new List<Task<string>>();
        }

        public void StartDownloadHtmlCss()
        {
            Download(Paths.HtmlCssRepositoriesFile, Paths.HtmlCssFolder);
        }

        public void StartDownloadJavaScript()
        {
            Download(Paths.JavaScriptRepositoriesFile, Paths.JavaScriptFolder);
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void Download(string repositoriesFileName, string folder)
        {
            downloads.Clear();
            string archiveName;
            string fullPath;
            foreach (string repository in RepositoriesReader.ReadRepositoreis(repositoriesFileName))
            {
                if (finished.Contains(repository)) continue;
                archiveName = repository.Replace('/', '-') + ".zip";
                fullPath = string.Format(folder, archiveName);
                downloads.Add(loader.DownloadZipAsync(repository, fullPath));
            }
            while (downloads.Count > 0 && !isStopped) // Check while condition
            {
                finishedTask = Task.WhenAny(downloads).Result;
                downloads.Remove(finishedTask);
                finished.Add(Patterns.GetRepositoryName(finishedTask.Result));
            }
            if(downloads.Count == 0) isFinished = true; // TODO: Add a check end of downloader work
        }
    }
}
