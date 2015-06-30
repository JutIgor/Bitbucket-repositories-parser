using System.ServiceProcess;
using System.Threading;

namespace DownloadService
{
    public partial class DownloadService : ServiceBase
    {
        private Thread threadHtmlCss;
        private Thread threadJavaScript;
        private DownloadRepositories downloaderHtmlCss = new DownloadRepositories();
        private DownloadRepositories downloaderJavaScript = new DownloadRepositories();

        public DownloadService()
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
