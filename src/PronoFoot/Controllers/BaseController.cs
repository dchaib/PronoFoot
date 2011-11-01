using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Authentication;
using PronoFoot.Data.Model;
using PronoFoot.Business.Contracts;

namespace PronoFoot.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUserService UserService;
        private User currentUser;

        public BaseController(IUserService userService)
        {
            if (userService == null) throw new ArgumentNullException("userService");
            this.UserService = userService;
        }

        protected int CurrentUserId
        {
            get { return this.User.GetPronoFootIdentity().UserId; }
        }

        protected User CurrentUser
        {
            get
            {
                return currentUser ??
                    (this.currentUser = this.UserService.GetUser(this.CurrentUserId));
            }
        }
    }
}