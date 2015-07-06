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
        [IgnoreDataMember]
        public bool isFinished;

        public void AllocateMemory()
        {
            this.downloads = new List<Task<string>>();
            this.loader = new Loader();
        }

        public void StartDownloadHtmlCss()
        {
            Download(Paths.HtmlCssRepositoriesFile, Paths.HtmlCssFolder, Patterns.HtmlCss);
        }

        public void StartDownloadJavaScript()
        {
            Download(Paths.JavaScriptRepositoriesFile, Paths.JavaScriptFolder, Patterns.JavaScript);
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void Download(string repositoriesFileName, string folder, string language)
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
            if (downloads.Count == 0)
            {
                isFinished = true; // TODO: Add a check end of downloader work
                Serializer.ClearDownloader(language);
            }
        }
    }
}
