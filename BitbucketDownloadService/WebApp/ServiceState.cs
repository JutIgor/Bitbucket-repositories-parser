using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DownloadService.WebApp
{
    public class ServiceState
    {
        public DateTime Time { get; set; }
        public string State { get; set; }
        public int DownloadedFiles { get; set; }

        public ServiceState GetServiceState()
        {
            return new ServiceState
            {
                Time = DateTime.Now,
                State = this.State,
                DownloadedFiles = RepositoriesDownloader.DownloadsCounter // TODO: Add serialization for counter
            };
        }
    }
}
