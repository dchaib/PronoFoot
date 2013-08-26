using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Contracts
{
    public interface IUserService
    {
        User GetUser(int userId);
        User GetUserByLogin(string login);
        IEnumerable<User> GetUsers();

        int Create(User user);
        void Update(User user);

        void ResetPassword();
    }
}
