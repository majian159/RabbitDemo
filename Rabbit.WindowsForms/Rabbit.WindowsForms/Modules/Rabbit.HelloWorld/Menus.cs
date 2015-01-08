using Rabbit.Core;
using Rabbit.Core.Menus;
using System.Collections.Generic;
using System.Windows.Forms;
using MenuItem = Rabbit.Core.Menus.MenuItem;

namespace Rabbit.HelloWorld
{
    internal sealed class Menus : IMenuProvider
    {
        #region Field

        private readonly IUserInterfaceController _userInterfaceController;

        #endregion Field

        #region Constructor

        public Menus(IUserInterfaceController userInterfaceController)
        {
            _userInterfaceController = userInterfaceController;
        }

        #endregion Constructor

        #region Implementation of IMenuProvider

        public void GetMenus(ICollection<MenuItem> menus)
        {
            menus.Add(new MenuItem
            {
                Text = "Hello World",
                Childs = new[]
                {
                    new MenuItem
                    {
                        Text = "WinForm",
                        ClickAction = () => _userInterfaceController.ShowContent("Hello World", new HelloWorld())
                    },
                    new MenuItem
                    {
                        Text = "MessageBox",
                        ClickAction = () => MessageBox.Show("Hello World!")
                    }
                }
            });
        }

        #endregion Implementation of IMenuProvider
    }
}