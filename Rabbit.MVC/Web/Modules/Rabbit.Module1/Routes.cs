using Rabbit.Web;
using Rabbit.Web.Routes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Rabbit.Module1
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
            const string area = "Rabbit.Module1";

            routes.MapRabbitRoute("Admin/Module1/{controller}/{action}/{id}", area, "Admin", "Index", new { id = UrlParameter.Optional });
        }

        #endregion Implementation of IRouteProvider
    }
}