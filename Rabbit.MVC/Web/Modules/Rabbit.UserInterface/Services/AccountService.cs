using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.UserInterface.Models;
using Rabbit.UserInterface.Util;
using System;
using System.Linq;

namespace Rabbit.UserInterface.Services
{
    /// <summary>
    /// 一个抽象的账号服务。
    /// </summary>
    public interface IAccountService : IDependency
    {
        /// <summary>
        /// 检查是否存在匹配的账号。
        /// </summary>
        /// <param name="account">账号字符串。</param>
        /// <param name="password">未加密的密码字符串。</param>
        /// <returns>如果存在则返回 true，否则返回 false。</returns>
        bool Exist(string account, string password);

        /// <summary>
        /// 检查账号是否存在。
        /// </summary>
        /// <param name="account">账号字符串。</param>
        /// <returns>如果存在则返回 true，否则返回 false。</returns>
        bool Exist(string account);
    }

    internal sealed class AccountService : IAccountService
    {
        #region Field

        private readonly Lazy<IRepository<AccountRecord>> _repository;

        #endregion Field

        #region Constructor

        public AccountService(Lazy<IRepository<AccountRecord>> repository)
        {
            _repository = repository;
        }

        #endregion Constructor

        #region Implementation of IAccountService

        /// <summary>
        /// 检查是否存在匹配的账号。
        /// </summary>
        /// <param name="account">账号字符串。</param>
        /// <param name="password">未加密的密码字符串。</param>
        /// <returns>如果存在则返回 true，否则返回 false。</returns>
        public bool Exist(string account, string password)
        {
            account = account.NotEmptyOrWhiteSpace("account").ToLower();
            password = EncryptHelper.Encrypt(password.NotEmptyOrWhiteSpace("password"));

            var table = _repository.Value.Table;
            return table.Any(i => i.Account == account && password == i.Password);
        }

        /// <summary>
        /// 检查账号是否存在。
        /// </summary>
        /// <param name="account">账号字符串。</param>
        /// <returns>如果存在则返回 true，否则返回 false。</returns>
        public bool Exist(string account)
        {
            account = account.NotEmptyOrWhiteSpace("account").ToLower();

            return _repository.Value.Table.Any(i => i.Account == account);
        }

        #endregion Implementation of IAccountService
    }
}