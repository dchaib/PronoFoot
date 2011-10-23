using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Business.Contracts
{
    public interface IUserService
    {
        User GetUser(int userId);
        User GetUserByLogin(string login);
    }
}
