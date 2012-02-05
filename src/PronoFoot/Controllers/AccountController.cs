using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using PronoFoot.Models;
using PronoFoot.Business.Contracts;
using PronoFoot.Data.Model;
using PronoFoot.Mvc.Extensions;

namespace PronoFoot.Controllers
{
    public class AccountController : BaseController
    {
        private readonly Security.IAuthenticationService authenticationService;
        private readonly Security.IMembershipService membershipService;

        public AccountController(IUserService userService, Security.IAuthenticationService authenticationService, Security.IMembershipService membershipService)
            : base(userService, authenticationService)
        {
            this.authenticationService = authenticationService;
            this.membershipService = membershipService;
        }

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (membershipService.ValidateUser(model.UserName, model.Password))
                {
                    var user = userService.GetUserByLogin(model.UserName);
                    authenticationService.SignIn(this.HttpContext, new Security.User { Id = user.UserId, Name = user.Login }, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "L'identifiant ou le mot de passe fourni est incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            authenticationService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        ////
        //// POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = membershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    var userId = userService.Create(new User { Login = model.UserName, Name = model.UserName, Email = model.Email });
                    authenticationService.SignIn(this.HttpContext, new Security.User { Id = userId, Name = model.UserName }, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = membershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Le mot de passe actuel est erroné ou le nouveau mot de passe n'est pas valide.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult RequestPasswordReset()
        {
            if (!membershipService.PasswordResetEnabled)
                return new HttpNotFoundResult();

            return View();
        }

        [HttpPost]
        public ActionResult RequestPasswordReset(string username)
        {
            if (!membershipService.PasswordResetEnabled)
                return new HttpNotFoundResult();

            if (String.IsNullOrWhiteSpace(username))
                return View();

            membershipService.SendRequestResetPasswordEmail(username, nonce => Url.ToPublicUrl(Url.Action("ResetPassword", "Account", new { nonce = nonce })));

            return RedirectToAction("RequestPasswordResetSuccess");
        }

        public ActionResult RequestPasswordResetSuccess()
        {
            return View();
        }

        public ActionResult ResetPassword(string nonce)
        {
            if (membershipService.ValidateResetPassword(nonce) == null)
            {
                return RedirectToAction("LogOn");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string nonce, ResetPasswordModel model)
        {
            string userName = membershipService.ValidateResetPassword(nonce);
            if (userName == null)
            {
                return RedirectToAction("LogOn");
            }

            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool resetPasswordSucceeded;
                try
                {
                    resetPasswordSucceeded = membershipService.ResetPassword(userName, model.NewPassword);
                }
                catch (Exception)
                {
                    resetPasswordSucceeded = false;
                }

                if (resetPasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "Le nouveau mot de passe n'est pas valide.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
