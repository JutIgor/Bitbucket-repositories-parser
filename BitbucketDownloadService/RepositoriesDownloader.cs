using RepositoriesDownloader;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System;
using System.IO;
using System.Net;

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

        public void Download(string repositoriesFileName, string folder, string language) // TODO: Check repositories name '-' '/'
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
                        //finished.Add(Patterns.GetRepositoryName(finishedTask.Result));
                        finished.Add(finishedTask.Result);
                        currentStreams--;
                    }
                    downloads.Add(loader.DownloadZipAsync(repository, fullPath));
                    currentStreams++;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException is WebException)
                    {
                        finished.Add(Patterns.GetRepositoryName(ex.InnerException.Message));
                        using (var writer = new StreamWriter(Paths.logName, true))
                        {
                            writer.WriteLine(ex.InnerException.Message); //ex.InnerException.Message
                        }
                    }
                    else
                    {
                        using (var writer = new StreamWriter(Paths.logName, true))
                        {
                            writer.WriteLine(ex.ToString());
                        }
                    }
                    currentStreams--;
                    continue;
                }
                //catch (Exception ex)
                //{
                //    using (var writer = new StreamWriter(logName, true))
                //    {
                //        writer.WriteLine(ex.GetType());
                //        writer.WriteLine("---");
                //        writer.WriteLine(ex.ToString());
                //    }
                //    currentStreams--;
                //    continue;
                //}
            }
            if (!isStopped)
            {
                Task.WaitAll(downloads.ToArray());
                Serializer.ClearDownloader(language);
            }
        }


        //foreach (string repository in RepositoriesReader.ReadRepositoreis(repositoriesFileName))
        //{
        //    if (finished.Contains(repository)) continue;
        //    archiveName = repository.Replace('/', '-') + ".zip";
        //    fullPath = string.Format(folder, archiveName);
        //    downloads.Add(loader.DownloadZipAsync(repository, fullPath));
        //}
        //while (downloads.Count > 0 && !isStopped) // Check while condition
        //{
        //    finishedTask = Task.WhenAny(downloads).Result;
        //    downloads.Remove(finishedTask);
        //    finished.Add(Patterns.GetRepositoryName(finishedTask.Result));
        //}
        //if (downloads.Count == 0)
        //{
        //    isFinished = true; // TODO: Add a check end of downloader work
        //    Serializer.ClearDownloader(language);
        //}

        //    var counter = 0;

        //    var tasks = allUris.Take(maxConcurrentStreams).Select(x => DownloadAsync(x)).ToArray();

        //    for (int i = maxConcurrentStreams; i < allUris.Count; i++)
        //    {
        //        counter = Task.WaitAny(tasks);
        //        yield return tasks[counter].Result;
        //        tasks[counter] = DownloadAsync(allUris[i]);
        //    }
        //    Task.WaitAll(tasks);
        //foreach (var task in tasks)
        //{
        //    yield return task.Result;
        //}
        //}
    }
}
