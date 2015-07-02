using Rabbit.Components.Security;
using System.Collections.Generic;

namespace Rabbit.Infrastructures.Security
{
    /// <summary>
    /// 用户模型。
    /// </summary>
    public class UserModel : IUser, IUserRoles
    {
        #region Implementation of IUser

        /// <summary>
        /// 用户标识。
        /// </summary>
        public string Identity { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        public string UserName { get; set; }

        #endregion Implementation of IUser

        #region Implementation of IUserRoles

        /// <summary>
        /// 用户所有角色。
        /// </summary>
        public IList<string> Roles { get { return new List<string> { "administrator" }; } }

        #endregion Implementation of IUserRoles
    }
}