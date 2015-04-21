using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Rabbit.ModuleManager
{
    internal sealed class AdminMenu : INavigationProvider
    {
        public AdminMenu()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        #region Implementation of INavigationProvider

        /// <summary>
        /// 获取导航。
        /// </summary>
        /// <param name="builder">导航建造者。</param>
        public void GetNavigation(NavigationBuilder builder)
        {
            const string area = "Rabbit.ModuleManager";
            builder.Add(T("系统管理"),
                menu =>
                    menu
                        .Position("1.1")
                        .LocalNavigation()
                        .Add(T("模块管理"), i => i.Action("Index", "Admin", new { Area = area }).LocalNavigation())
                );
        }

        /// <summary>
        /// 导航菜单名称。
        /// </summary>
        public string MenuName { get { return "admin"; } }

        #endregion Implementation of INavigationProvider
    }
}