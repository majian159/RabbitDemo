using Autofac;
using Rabbit.Components.Logging.NLog;
using Rabbit.Components.Security.Web;
using Rabbit.Infrastructures;
using Rabbit.Kernel;
using Rabbit.Kernel.Caching;
using Rabbit.Kernel.Caching.Impl;
using Rabbit.Kernel.Logging;
using Rabbit.Web;
using Rabbit.Web.Environment;
using Rabbit.Web.Mvc;
using Rabbit.Web.WarmupStarter;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class MvcApplication : HttpApplication
    {
        private static Starter<IWebHost> _starter;

        protected void Application_Start()
        {
            RegisterRoutes(RouteTable.Routes);
            _starter = new Starter<IWebHost>(HostInitialization, HostBeginRequest, HostEndRequest);
            _starter.OnApplicationStart(this);
        }

        #region Event

        protected void Application_BeginRequest()
        {
            _starter.OnBeginRequest(this);
        }

        protected void Application_EndRequest()
        {
            _starter.OnEndRequest(this);
        }

        protected void Session_Start()
        {
            Session[Guid.NewGuid().ToString("N")] = null;
        }

        #endregion Event

        #region Private Method

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
        }

        private static void HostBeginRequest(HttpApplication application, IWebHost host)
        {
            application.Context.Items["originalHttpContext"] = application.Context;
            host.BeginRequest();
        }

        private static void HostEndRequest(HttpApplication application, IWebHost host)
        {
            host.EndRequest();
        }

        private static IWebHost HostInitialization(HttpApplication application)
        {
            var kernelBuilder = new KernelBuilder();

            kernelBuilder.UseCaching(c => c.UseMemoryCache());
            kernelBuilder.UseLogging(c => c.UseNLog());
            kernelBuilder.UseWeb(c => c.EnableMvc().EnableSecurity());

            //开启基础设置层。
            kernelBuilder.UseInfrastructures();

            var container = kernelBuilder.Build();

            var host = container.Resolve<IWebHost>();

            host.Initialize();
            host.BeginRequest();
            host.EndRequest();

            return host;
        }

        #endregion Private Method
    }
}