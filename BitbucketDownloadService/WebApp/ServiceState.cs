using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadService.WebApp
{
    public class ServiceState
    {
        public string Time { get; set; }
        public string State { get; set; }
        public int DownloadedFiles { get; set; } 

        public ServiceState GetServiceState()
        {
            return new ServiceState
            {
                Time = Clock.GetTime(),
                State = this.State,
                DownloadedFiles = RepositoriesDownloader.DownloadsCounter
            };
        }
    }
}
