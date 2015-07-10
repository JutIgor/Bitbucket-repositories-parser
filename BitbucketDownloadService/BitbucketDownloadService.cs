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

        public BitbucketDownloadService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
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
    }
}
