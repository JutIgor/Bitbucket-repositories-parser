using RepositoriesDownloader;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DownloadService
{
    public class DownloadRepositories
    {
        private List<string> finished = new List<string>();
        private List<Task<string>> downloads = new List<Task<string>>();
        private Loader loader = new Loader();
        private Task<string> finishedTask;
        private bool isStopped = false;

        public void StartDownloadHtmlCss()
        {
            Download(Pathes.HtmlCssRepositoriesFile, Pathes.HtmlCssFolder);
        }

        public void StartDownloadJavaScript()
        {
            Download(Pathes.JavaScriptRepositoriesFile, Pathes.JavaScriptFolder);
        }

        public void Stop()
        {
            isStopped = true;
        }

        public void Download(string repositoriesFileName, string folder)
        {
            string archiveName;
            foreach (string repository in RepositoriesReader.ReadRepositoreis(repositoriesFileName))
            {
                archiveName = repository.Replace('/', '-') + ".zip";
                string.Format(folder, archiveName);
                downloads.Add(loader.DownloadZipAsync(repository, folder));
            }
            while (downloads.Count > 0 && !isStopped) // Check while condition
            {
                finishedTask = Task.WhenAny(downloads).Result;
                downloads.Remove(finishedTask);
                finished.Add(finishedTask.Result);
            }
        }
    }
}
