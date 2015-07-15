using DownloadService.WebApp;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;

namespace DownloadService
{
    public partial class BitbucketDownloadService : ServiceBase
    {
        private Thread threadHtmlCss;
        private Thread threadJavaScript;
        private RepositoriesDownloader downloaderHtmlCss;
        private RepositoriesDownloader downloaderJavaScript;
        private IDisposable webApp;
        private ServiceState state;

        public BitbucketDownloadService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var endpoint = string.Format("{0}://{1}",
                ConfigurationManager.AppSettings["EndpointProtocol"],
                ConfigurationManager.AppSettings["EndpointPort"]);

            state = new ServiceState
            {
                State = "Service is running",
                Time = Clock.GetTime(),
                DownloadedFiles = 0
            };

            webApp = Microsoft.Owin.Hosting.WebApp.Start(endpoint, appBuilder => new Startup().Configuration(appBuilder, state));

            StartThreads();
        }

        protected override void OnStop()
        {
            StopThreads();
        }

        protected override void OnPause()
        {
            StopThreads();
        }

        protected override void OnContinue()
        {
            StartThreads();
        }

        protected override void OnShutdown()
        {
            StopThreads();
        }

        private void StartThreads()
        {
            DeserializeDownloaders();
            threadHtmlCss = new Thread(downloaderHtmlCss.StartDownloadHtmlCss);
            threadJavaScript = new Thread(downloaderJavaScript.StartDownloadJavaScript);
            threadHtmlCss.Start();
            threadJavaScript.Start();
        }

        private void StopThreads()
        {
            downloaderHtmlCss.Stop();
            downloaderJavaScript.Stop();
            SerializeDownloaders();
            threadHtmlCss.Abort();
            threadJavaScript.Abort();
        }

        private void SerializeDownloaders()
        {
            Serializer.SerializeDownloader(downloaderHtmlCss, Patterns.HtmlCss);
            Serializer.SerializeDownloader(downloaderJavaScript, Patterns.JavaScript);
        }

        private void DeserializeDownloaders()
        {
            downloaderHtmlCss = Serializer.DeserializeDownloader(Patterns.HtmlCss);
            downloaderJavaScript = Serializer.DeserializeDownloader(Patterns.JavaScript);
        }

        public new void Dispose()
        {
            if (webApp != null)
                webApp.Dispose();
        }
    }
}
