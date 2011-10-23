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
        protected readonly IUserService UserServices;
        private User currentUser;

        public BaseController(IUserService userServices)
        {
            if (userServices == null) throw new ArgumentNullException("userServices");
            this.UserServices = userServices;
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
                    (this.currentUser = this.UserServices.GetUser(this.CurrentUserId));
            }
        }
    }
}