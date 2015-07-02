using Rabbit.Components.Data;
using Rabbit.Kernel;
using Rabbit.Kernel.Utility.Extensions;
using Rabbit.UserInterface.Models;
using System;
using System.Linq;
using System.Text;

namespace Rabbit.UserInterface.Services
{
    public interface IAccountService : IDependency
    {
        bool Check(string account, string password);

        bool Exist(string account);
    }

    internal sealed class AccountService : IAccountService
    {
        private readonly Lazy<IRepository<AccountRecord>> _repository;

        public AccountService(Lazy<IRepository<AccountRecord>> repository)
        {
            _repository = repository;
        }

        #region Implementation of IAccountService

        public bool Check(string account, string password)
        {
            account = account.NotEmptyOrWhiteSpace("account").ToLower();
            password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password.NotEmptyOrWhiteSpace("password")));

            var table = _repository.Value.Table;
            return table.Any(i => i.Account == account && password == i.Password);
        }

        public bool Exist(string account)
        {
            account = account.NotEmptyOrWhiteSpace("account").ToLower();

            return _repository.Value.Table.Any(i => i.Account == account);
        }

        #endregion Implementation of IAccountService
    }
}