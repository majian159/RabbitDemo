using Rabbit.Kernel;
using System.Collections.Generic;

namespace Rabbit.Core.Menus
{
    public interface IMenuProvider : IDependency
    {
        void GetMenus(ICollection<MenuItem> menus);
    }
}