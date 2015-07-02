using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.UserInterface.Models;
using System;

namespace Rabbit.UserInterface.Services
{
    /// <summary>
    /// 一个抽象的用户服务。
    /// </summary>
    public interface IUserService : IDependency
    {
        /// <summary>
        /// 创建一个用户。
        /// </summary>
        /// <param name="user">用户记录。</param>
        void Create(UserRecord user);
    }

    internal sealed class UserService : IUserService
    {
        #region Field

        private readonly Lazy<IRepository<UserRecord>> _repository;

        #endregion Field

        #region Constructor

        public UserService(Lazy<IRepository<UserRecord>> repository)
        {
            _repository = repository;
        }

        #endregion Constructor

        #region Implementation of IUserService

        /// <summary>
        /// 创建一个用户。
        /// </summary>
        /// <param name="user">用户记录。</param>
        public void Create(UserRecord user)
        {
            _repository.Value.Create(user.NotNull("user"));
        }

        #endregion Implementation of IUserService
    }
}