using RepositoriesDownloader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DownloadService
{
    [DataContract(Name = "RepositoriesDownloader")]
    public class RepositoriesDownloader
    {
        [DataMember]
        private List<string> finished = new List<string>();
        [IgnoreDataMember]
        private List<string> cancelledDownloads = new List<string>();
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
        [DataMember]
        public static int DownloadsCounter;

        public void AllocateMemory()
        {
            this.downloads = new List<Task<string>>();
            this.cancelledDownloads = new List<string>();
            this.loader = new Loader();
            DownloadsCounter += finished.Count;
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
            foreach (var item in cancelledDownloads)
            {
                using (var writer = new StreamWriter(Paths.logName, true))
                {
                    writer.WriteLine(item);
                }
            }
        }

        public void Download(string repositoriesFileName, string folder, string language)
        {
            downloads.Clear();
            string archiveName;
            string fullPath;
            int currentStreams = 0;
            int maxStreams = 2;
            foreach (string repository in RepositoriesReader.ReadRepositoreis(repositoriesFileName))
            {
                try
                {
                    if (isStopped) break;
                    if (finished.Contains(repository)) continue;
                    archiveName = repository.Replace('/', '-') + ".zip";
                    fullPath = string.Format(folder, archiveName);
                    if (currentStreams == maxStreams)
                    {
                        finishedTask = Task.WhenAny(downloads).Result;
                        downloads.Remove(finishedTask);
                        finished.Add(finishedTask.Result);
                        currentStreams--;
                        DownloadsCounter++;
                    }
                    downloads.Add(loader.DownloadZipAsync(repository, fullPath));
                    currentStreams++;
                }
                catch (AggregateException ex)
                {
                    finished.Add(Patterns.GetRepositoryName(ex.InnerException.Message));
                    cancelledDownloads.Add(ex.InnerException.Message);
                    currentStreams--;
                    continue;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    currentStreams--;
                    continue;
                }
            }
            if (!isStopped)
            {
                Task.WaitAll(downloads.ToArray());
                Serializer.ClearDownloader(language);
            }
        }
    }
}
