using Rabbit.Components.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.UserInterface.Models
{
    /// <summary>
    /// 用户记录。
    /// </summary>
    [Entity]
    public class UserRecord
    {
        /// <summary>
        /// 标识。
        /// </summary>
        [Required, StringLength(32)]
        public string Id { get; set; }

        /// <summary>
        /// 账号记录。
        /// </summary>
        public virtual AccountRecord Account { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        [Required, StringLength(20)]
        public string Name { get; set; }

        /// <summary>
        /// 创建一个用户记录。
        /// </summary>
        /// <param name="name">用户名称。</param>
        /// <param name="account">账号记录。</param>
        /// <returns></returns>
        public static UserRecord Create(string name, AccountRecord account)
        {
            return new UserRecord
            {
                Id = Guid.NewGuid().ToString("N"),
                Account = account,
                Name = name
            };
        }
    }
}