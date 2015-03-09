using Rabbit.Web;
using Rabbit.Web.Routes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rabbit.UserInterface
{
    internal sealed class Routes : IRouteProvider
    {
        #region Implementation of IRouteProvider

        /// <summary>
        /// 获取路由信息。
        /// </summary>
        /// <param name="routes">路由集合。</param>
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            const string area = "Rabbit.UserInterface";
            routes.MapRabbitRoute("Admin", area, "Admin", "Index", new { id = UrlParameter.Optional });
            routes.MapRabbitRoute("Admin/UserInterface/{controller}/{action}/{id}", area, "Admin", "Index", new { id = UrlParameter.Optional });
            routes.MapRabbitRoute("{controller}/{action}/{id}", area, "Home", "Index", new { id = UrlParameter.Optional });
        }

        #endregion Implementation of IRouteProvider
    }
}