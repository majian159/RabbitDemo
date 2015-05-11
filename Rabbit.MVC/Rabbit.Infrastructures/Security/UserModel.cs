using Rabbit.Components.Security;

namespace Rabbit.Infrastructures.Security
{
    /// <summary>
    /// 用户模型。
    /// </summary>
    public class UserModel : IUser
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
    }
}