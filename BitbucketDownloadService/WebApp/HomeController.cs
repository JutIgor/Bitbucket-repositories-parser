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
        private ServiceState serviceState;

        public HomeController(ServiceState state)
        {
            serviceState = state;
        }

        public ServiceState Get()
        {
            return serviceState.GetServiceState();
        }
    }
}
