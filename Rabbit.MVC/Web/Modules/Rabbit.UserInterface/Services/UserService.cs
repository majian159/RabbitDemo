using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.UserInterface.Models;
using System;

namespace Rabbit.UserInterface.Services
{
    public interface IUserService : IDependency
    {
        void Create(UserRecord record);
    }

    internal sealed class UserService : IUserService
    {
        private readonly Lazy<IRepository<UserRecord>> _repository;

        public UserService(Lazy<IRepository<UserRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IUserService

        public void Create(UserRecord record)
        {
            _repository.Value.Create(record);
        }

        #endregion Implementation of IUserService
    }
}