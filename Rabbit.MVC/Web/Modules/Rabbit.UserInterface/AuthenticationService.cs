using Rabbit.Components.Security;
using Rabbit.Components.Security.Web;
using Rabbit.Kernel.Environment.Configuration;
using Rabbit.Kernel.Extensions;
using Rabbit.Kernel.Services;
using Rabbit.Web;

namespace Rabbit.UserInterface
{
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

    [SuppressDependency("Rabbit.Components.Security.NullAuthenticationService")]
    internal sealed class AuthenticationService : FormsAuthenticationServiceBase
    {
        /// <summary>
        /// 初始化一个表单授权服务。
        /// </summary>
        /// <param name="settings">租户设置。</param><param name="clock">时钟服务。</param><param name="httpContextAccessor">HttpContext访问器。</param>
        public AuthenticationService(ShellSettings settings, IClock clock, IHttpContextAccessor httpContextAccessor)
            : base(settings, clock, httpContextAccessor)
        {
        }

        #region Overrides of FormsAuthenticationServiceBase

        /// <summary>
        /// 根据用户标识获取用户信息。
        /// </summary>
        /// <param name="identity">用户标识。</param>
        /// <returns>
        /// 用户模型。
        /// </returns>
        protected override IUser GetUserByIdentity(string identity)
        {
            return new UserModel
            {
                Identity = identity,
                UserName = identity
            };
        }

        #endregion Overrides of FormsAuthenticationServiceBase
    }
}