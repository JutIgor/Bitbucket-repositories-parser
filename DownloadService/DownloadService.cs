using System.ServiceProcess;
using System.Threading;

namespace DownloadService
{
    public partial class DownloadService : ServiceBase
    {
        private Thread threadHtmlCss;
        private Thread threadJavaScript;
        private RepositoriesDownloader downloaderHtmlCss;
        private RepositoriesDownloader downloaderJavaScript;

        public DownloadService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            downloaderHtmlCss = new RepositoriesDownloader();
            downloaderJavaScript = new RepositoriesDownloader();
            StartThreads();
        }

        protected override void OnStop()
        {
            StopThreads();
        }

        protected override void OnPause()
        {
            Serializer.SerializeDownloader(downloaderHtmlCss, Patterns.HtmlCss);
            Serializer.SerializeDownloader(downloaderJavaScript, Patterns.JavaScript);
            StopThreads();
        }

        protected override void OnContinue()
        {
            downloaderHtmlCss = Serializer.DeserializeDownloader(Patterns.HtmlCss);
            downloaderJavaScript = Serializer.DeserializeDownloader(Patterns.JavaScript);
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        private void StartThreads()
        {
            threadHtmlCss = new Thread(downloaderHtmlCss.StartDownloadHtmlCss);
            threadJavaScript = new Thread(downloaderJavaScript.StartDownloadJavaScript);
        }

        private void StopThreads()
        {
            downloaderHtmlCss.Stop();
            downloaderJavaScript.Stop();
            threadHtmlCss.Join();
            threadJavaScript.Join();
        }
    }
}
