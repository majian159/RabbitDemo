using Rabbit.Components.Security.Web;
using System.Collections.Generic;

namespace Rabbit.Infrastructures.Adapter
{
    internal sealed class RoleService : IRoleService
    {
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
            return new[] { StandardPermissions.Owner.Name };
        }

        #endregion Implementation of IRoleService
    }
}