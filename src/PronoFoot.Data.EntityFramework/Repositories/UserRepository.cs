using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Data.Model;

namespace PronoFoot.Data.EntityFramework.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public User GetUser(int userId)
        {
            return this.GetDbSet<User>()
                .Where(x => x.UserId == userId)
                .Single();
        }

        public User GetUserByLogin(string login)
        {
            return this.GetDbSet<User>()
                .Where(x => x.Login == login)
                .Single();
        }

        public IEnumerable<User> GetUsers()
        {
            return this.GetDbSet<User>()
                .ToList();
        }

        public int Create(User user)
        {
            this.GetDbSet<User>().Add(user);
            this.UnitOfWork.SaveChanges();
            return user.UserId;
        }
    }
}
