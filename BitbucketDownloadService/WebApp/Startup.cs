using DownloadService.Infrastructure;
using Ninject;
using Owin;
using System.Web.Http;

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
