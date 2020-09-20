using NLog;
using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcMusicStore.Interfaces;
using MvcMusicStore.Infrastructure;
using PerformanceCounterHelper;
using System.Configuration;
using MvcMusicStore.Models;
using System.Web.WebPages;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILogger_MusicStore logger;
        
        public MvcApplication()
        {
            if (ConfigurationManager.AppSettings["Logging"].AsBool())
            {
                logger = new MvcMusicStore.Models.Logger();
            }
        }
        protected void Application_Start()
        {

            ContainerBuilder builder  = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.Register(c => PerformanceHelper.CreateCounterHelper<Counters>()).SingleInstance().As<CounterHelper<Counters>>();
            builder.RegisterType<Models.Logger>().As<ILogger_MusicStore>();
            var container = builder.Build();
            var resolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(resolver);


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.LogInfo("Application started");
            
           
        }
        protected void Application_Error()
        {
            Exception exception = Server.GetLastError();
            var logger = DependencyResolver.Current.GetService(typeof(ILogger_MusicStore)) as ILogger_MusicStore;
            logger?.LogError(exception, exception.Message);
        }
    }
}
