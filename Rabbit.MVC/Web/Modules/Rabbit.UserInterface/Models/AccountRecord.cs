using Rabbit.Components.Data;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.UserInterface.Util;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.UserInterface.Models
{
    /// <summary>
    /// 账号记录。
    /// </summary>
    [Entity]
    public class AccountRecord
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Required, StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        /// 账号。
        /// </summary>
        [Required, StringLength(20)]
        public string Account { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Required, StringLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 设置密码。
        /// </summary>
        /// <param name="password">密码字符串。</param>
        /// <returns>账号记录。</returns>
        public AccountRecord SetPassword(string password)
        {
            password.NotEmptyOrWhiteSpace("password");

            Password = EncryptHelper.Encrypt(password);
            return this;
        }

        /// <summary>
        /// 创建一个新的账号记录。
        /// </summary>
        /// <param name="account">账号字符串。</param>
        /// <param name="password">密码字符串。</param>
        /// <returns>账号记录。</returns>
        public static AccountRecord Create(string account, string password)
        {
            return new AccountRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Account = account.NotEmptyOrWhiteSpace("account").ToLower()
            }.SetPassword(password.NotEmptyOrWhiteSpace("password"));
        }
    }
}