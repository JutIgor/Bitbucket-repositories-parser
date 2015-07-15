using System.ServiceProcess;

namespace DownloadService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new BitbucketDownloadService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
