using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Data.Model;
using PronoFoot.Data;

namespace PronoFoot.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            this.userRepository = userRepository;
        }

        public User GetUser(int userId)
        {
            User user = this.userRepository.GetUser(userId);
            return user;
        }

        public User GetUserByLogin(string login)
        {
            //if (login == null)
            //{
            //    throw new BusinessServicesException(Resources.UnableToFindUserExceptionMessage,
            //                                        new ArgumentNullException("login"));
            //}
            //if (String.IsNullOrEmpty(login))
            //{
            //    throw new BusinessServicesException(Resources.UnableToFindUserExceptionMessage,
            //                                        new ArgumentException("login"));
            //}

            try
            {
                User user = this.userRepository.GetUserByLogin(login);
                if (user != null)
                {
                    return user;
                }
                return null;
            }
            catch (InvalidOperationException ex)
            {
                //throw new BusinessServicesException(Resources.UnableToFindUserExceptionMessage, ex);
                throw ex;
            }
        }
    }
}
