﻿using RepositoriesDownloader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

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
        [IgnoreDataMember]
        private CancellationTokenSource cts = new CancellationTokenSource();

        public void AllocateMemory()
        {
            this.downloads = new List<Task<string>>();
            this.loader = new Loader();
            this.cts = new CancellationTokenSource();
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
                    if (isStopped)
                    {
                        if (cts != null) cts.Cancel();
                        break;
                    }
                    if (finished.Contains(repository)) continue;
                    archiveName = repository.Replace('/', '-') + ".zip";
                    fullPath = string.Format(folder, archiveName);
                    if (currentStreams == maxStreams)
                    {
                        finishedTask = Task.WhenAny(downloads).Result;
                        downloads.Remove(finishedTask);
                        finished.Add(finishedTask.Result);
                        currentStreams--;
                    }
                    downloads.Add(loader.DownloadZipAsync(repository, fullPath, cts.Token));
                    currentStreams++;
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerException is WebException)
                    {
                        finished.Add(Patterns.GetRepositoryName(ex.InnerException.Message));
                        using (var writer = new StreamWriter(Paths.logName, true))
                        {
                            writer.WriteLine(ex.InnerException.Message);
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
            }
            if (!isStopped)
            {
                Task.WaitAll(downloads.ToArray());
                Serializer.ClearDownloader(language);
            }
        }
    }
}
