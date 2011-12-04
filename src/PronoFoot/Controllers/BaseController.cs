using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PronoFoot.Data.Model;
using PronoFoot.Business.Contracts;
using PronoFoot.Security;

namespace PronoFoot.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUserService userService;
        private readonly IAuthenticationService authenticationService;
        private Data.Model.User currentUser;

        public BaseController(IUserService userService, IAuthenticationService authenticationService)
        {
            if (userService == null) throw new ArgumentNullException("userService");
            if (authenticationService == null) throw new ArgumentNullException("authenticationService");
            this.userService = userService;
            this.authenticationService = authenticationService;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (this.CurrentUser != null)
                ViewBag.UserName = this.CurrentUser.Name;
        }

        protected Data.Model.User CurrentUser
        {
            get
            {
                if (currentUser != null)
                    return currentUser;

                Security.User authenticatedUser = authenticationService.GetAuthenticatedUser(this.HttpContext);
                if (authenticatedUser == null)
                    return null;

                currentUser = userService.GetUser(authenticatedUser.Id);
                return currentUser;
            }
        }
    }
}