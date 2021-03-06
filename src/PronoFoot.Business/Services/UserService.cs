﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PronoFoot.Business.Contracts;
using PronoFoot.Data.Model;
using PronoFoot.Data;
using PronoFoot.Business.Models;

namespace PronoFoot.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IForecastRepository forecastRepository;

        public UserService(IUserRepository userRepository, IForecastRepository forecastRepository)
        {
            if (userRepository == null) throw new ArgumentNullException("userRepository");
            this.userRepository = userRepository;
            this.forecastRepository = forecastRepository;
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

        public IEnumerable<User> GetUsers()
        {
            return this.userRepository.GetUsers();
        }

        public int Create(User user)
        {
            return this.userRepository.Create(user);
        }

        public void Update(User user)
        {
            this.userRepository.Update(user);
        }

        public void ResetPassword()
        {

        }        
    }
}
