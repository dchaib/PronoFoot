using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data
{
    public interface IUserRepository
    {
        User GetUser(int userId);
        User GetUserByLogin(string login);
    }
}
