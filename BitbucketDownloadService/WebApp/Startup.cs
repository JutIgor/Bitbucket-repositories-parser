using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Ninject;
using DownloadService.Infrastructure;

namespace DownloadService.WebApp
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder, ServiceState state)
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}",
                defaults: new { controller = RouteParameter.Optional }
                );
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            var kernel = new StandardKernel();
            kernel.Bind(state.GetType()).ToConstant(state).InThreadScope();
            config.DependencyResolver = new NinjectDependencyResolver(kernel);
            appBuilder.UseWebApi(config);
        }
    }
}
