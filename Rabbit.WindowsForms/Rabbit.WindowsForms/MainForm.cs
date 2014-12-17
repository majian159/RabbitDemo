using Rabbit.Core;
using Rabbit.Core.Menus;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MenuItem = Rabbit.Core.Menus.MenuItem;

namespace Rabbit.WindowsForms
{
    public partial class MainForm : Form, IUserInterfaceController
    {
        #region Field

        private readonly Lazy<IEnumerable<IMenuProvider>> _menuProviders;

        #endregion Field

        #region Constructor

        public MainForm(Lazy<IEnumerable<IMenuProvider>> menuProviders)
        {
            _menuProviders = menuProviders;
            InitializeComponent();
        }

        #endregion Constructor

        #region Implementation of IMainController

        public void ShowContent(string title, Control control)
        {
            var tabPage = tabControl.TabPages.Cast<TabPage>().FirstOrDefault(i => i.Name == control.Name);
            if (tabPage == null)
            {
                tabPage = new TabPage(title)
                {
                    Name = control.Name
                };
                if (control is Form)
                {
                    var form = control as Form;
                    form.TopLevel = false;
                    form.Show();
                    form.FormBorderStyle = FormBorderStyle.None;
                }
                tabPage.Controls.Add(control);
                tabControl.TabPages.Add(tabPage);
            }
            tabControl.SelectTab(tabPage);
        }

        #endregion Implementation of IMainController

        #region Event

        private void MainForm_Load(object sender, EventArgs e)
        {
            var menuList = new List<MenuItem>();
            foreach (var menuProvider in _menuProviders.Value)
            {
                menuProvider.GetMenus(menuList);
            }

            AddMenus(menuStrip.Items, menuList);
        }

        private void contextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var selectedTab = tabControl.SelectedTab;
            switch (e.ClickedItem.Name)
            {
                case "tsmiClose":
                    TabPage previousTabPage = null;

                    foreach (var tabPage in tabControl.TabPages.Cast<TabPage>().TakeWhile(tabPage => selectedTab != tabPage))
                        previousTabPage = tabPage;

                    tabControl.TabPages.Remove(tabControl.SelectedTab);
                    if (previousTabPage != null)
                    {
                        tabControl.SelectTab(previousTabPage);
                    }
                    break;

                case "tsmiCloseOther":
                    foreach (var tabPage in tabControl.TabPages.Cast<TabPage>().Where(i => i != tabControl.SelectedTab).ToArray())
                        tabControl.TabPages.Remove(tabPage);
                    break;
            }
        }

        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;

            var tabCount = tabControl.TabCount;
            var enabled = tabCount > 1;
            foreach (ToolStripMenuItem item in contextMenuStrip.Items)
                item.Enabled = enabled;

            for (var i = 0; i < tabCount; i++)
            {
                if (!tabControl.GetTabRect(i).Contains(new Point(e.X, e.Y)))
                    continue;
                tabControl.SelectTab(i);
                contextMenuStrip.Show(MousePosition.X, MousePosition.Y);
                break;
            }
        }

        #endregion Event

        #region Private Method

        private void AddMenus(ToolStripItemCollection toolStripItemCollection, IEnumerable<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                var item = new ToolStripMenuItem(menuItem.Text);
                toolStripItemCollection.Add(item);
                if (menuItem.Childs != null && menuItem.Childs.Any())
                    AddMenus(item.DropDownItems, menuItem.Childs);
                if (menuItem.ClickAction == null)
                    continue;
                var itemProxy = menuItem;
                item.Click += (s, e) => itemProxy.ClickAction();
            }
        }

        #endregion Private Method
    }
}