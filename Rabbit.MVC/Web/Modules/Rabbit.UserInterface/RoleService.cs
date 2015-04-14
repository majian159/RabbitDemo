using Rabbit.Components.Security.Permissions;
using Rabbit.Components.Security.Web;
using System.Collections.Generic;
using System.Linq;

namespace Rabbit.UserInterface
{
    internal sealed class RoleService : IRoleService
    {
        #region Field

        private readonly IEnumerable<IPermissionProvider> _providers;

        #endregion Field

        #region Constructor

        public RoleService(IEnumerable<IPermissionProvider> providers)
        {
            _providers = providers;
        }

        #endregion Constructor

        #region Implementation of IRoleService

        /// <summary>
        /// 根级角色的名称获取该角色所拥有的权限。
        /// </summary>
        /// <param name="name">角色名称。</param>
        /// <returns>
        /// 该角色所拥有的权限。
        /// </returns>
        public IEnumerable<string> GetPermissionsForRoleByName(string name)
        {
            //返回所有权限。
            return _providers.SelectMany(i => i.GetPermissions().Select(z => z.Name)).ToArray();
        }

        #endregion Implementation of IRoleService
    }
}