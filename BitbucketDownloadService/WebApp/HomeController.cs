using System.Web.Http;

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
