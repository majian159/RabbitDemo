using Rabbit.Kernel.Localization;
using Rabbit.Web.UI.Navigation;

namespace Rabbit.UserInterface
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
            const string area = "Rabbit.UserInterface";
            builder.Add(T("控制台"),
                menu =>
                    menu
                        .Position("1")
                        .LocalNavigation()
                        .Icon("fa fa-fw fa-dashboard")
                        .Add(T("主页"), i => i.Action("Index", "Admin", new { Area = area }).LocalNavigation())
                        .Add(T("会员CRM"), i => i.Action("Index", "MemberCrm", new { Area = area }).LocalNavigation()
                            .Add(T("编辑会员资料"), z => z.Action("Edit", "MemberCrm", new { Area = area })))
                        .Add(T("会员卡设置"), i => i.Action("Index", "MemberCardSetting", new { Area = area }).LocalNavigation()
                            .Add(T("添加特权"), z => z.Action("AddPrivilege", "MemberCardSetting", new { Area = area }))
                            .Add(T("编辑特权"), z => z.Action("EditPrivilege", "MemberCardSetting", new { Area = area })))
                        .Add(T("会员卡管理"), i => i.Action("Index", "MemberCardAdmin", new { Area = area }).LocalNavigation()
                            .Add(T("编辑会员卡资料"), z => z.Action("Edit", "MemberCardAdmin", new { Area = area })))
                );
        }

        /// <summary>
        /// 导航菜单名称。
        /// </summary>
        public string MenuName { get { return "admin"; } }

        #endregion Implementation of INavigationProvider
    }
}