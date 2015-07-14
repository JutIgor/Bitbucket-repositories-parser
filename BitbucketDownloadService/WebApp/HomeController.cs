using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace DownloadService.WebApp
{
    public class HomeController : ApiController
    {
        private ServiceState state = new ServiceState();

        public ServiceState Get()
        {
            return state.GetServiceState();
        }
    }
}
