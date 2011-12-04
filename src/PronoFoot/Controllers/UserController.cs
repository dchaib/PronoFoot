using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PronoFoot.Business.Contracts;
using PronoFoot.ViewModels;

namespace PronoFoot.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IUserService userService, Security.IAuthenticationService authenticationService)
            : base(userService, authenticationService)
        {
        }

        //
        // GET: /User/
        [Authorize]
        public ActionResult Index()
        {
            var currentUser = this.CurrentUser;
            return View(currentUser);
        }

        //
        // GET: /User/Edit
        [Authorize]
        public ActionResult Edit()
        {
            var currentUser = this.CurrentUser;
            var viewModel = new UserEditViewModel
            {
                Name = currentUser.Name,
                Email = currentUser.Email
            };
            return View(viewModel);
        }

        //
        // POST: /User/Edit
        [Authorize]
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                user.Email = model.Email;
                Membership.UpdateUser(user);

                var currentUser = this.CurrentUser;
                currentUser.Name = model.Name;
                currentUser.Email = model.Email;
                userService.Update(currentUser);

                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
