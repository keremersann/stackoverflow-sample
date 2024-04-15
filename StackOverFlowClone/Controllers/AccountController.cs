using StackOverFlowClone.CustomFilters;
using StackOverFlowClone.DomainModels;
using StackOverFlowClone.ServiceLayer;
using StackOverFlowClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlowClone.Controllers
{
    public class AccountController : Controller
    {
        IUsersService us;
        public AccountController(IUsersService us)
        {
            this.us = us;
        }
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            if (ModelState.IsValid)
            {
                int userId = us.InsertUser(rvm);
                Session["CurrentUserId"] = userId;
                Session["CurrentUserName"] = rvm.Name;
                Session["CurrentUserEmail"] = rvm.Email;
                Session["CurrentUserPassword"] = rvm.Password;
                Session["CurrentUserMobile"] = rvm.Mobile;
                Session["CurrentUserIsAdmin"] = false;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View();
            }
        }
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var userViewModel = us.GetUsersByEmailAndPassword(lvm.Email, lvm.Password);
                if (userViewModel != null)
                {
                    Session["CurrentUserId"] = userViewModel.UserID;
                    Session["CurrentUserName"] = userViewModel.Name;
                    Session["CurrentUserEmail"] = userViewModel.Email;
                    Session["CurrentUserPassword"] = userViewModel.Password;
                    Session["CurrentUserMobile"] = userViewModel.Mobile;
                    Session["CurrentUserIsAdmin"] = userViewModel.IsAdmin;

                    if (userViewModel.IsAdmin)
                        return RedirectToRoute(new { area = "admin", action = "Index", controller = "AdminHome" });
                    else
                        return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("x", "Invalid Email / Password");
                    return View(lvm);
                }
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return View(lvm);
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
       [UserAuthorizationFilter]
        public ActionResult ChangeProfile()
        {
            var userId = Convert.ToInt32(Session["CurrentUserId"]);
            var user = us.GetUsersById(userId);
            var editUserViewModel = new EditUserDetailsViewModel() { Name = user.Name, Email = user.Email, Mobile = user.Mobile, UserID = user.UserID };
            return View(editUserViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult ChangeProfile(EditUserDetailsViewModel eudvm)
        {
            if (ModelState.IsValid)
            {
                var user = us.GetUsersById(eudvm.UserID);
                if (user != null)
                {
                    us.UpdateUserDetails(eudvm);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("x", "User is not found!");
                    return View(eudvm);
                }
            }
            else
            {
                ModelState.AddModelError("x", "Incorrect User Details!");
                return View(eudvm);
            }
        }
        [UserAuthorizationFilter]
        public ActionResult ChangePassword()
        {
            int userId = Convert.ToInt32(Session["CurrentUserId"]);
            var user = us.GetUsersById(userId);
            EditUserPasswordViewModel editUserPasswordViewModel = null;
            if(user != null)
            {
                editUserPasswordViewModel = new EditUserPasswordViewModel() { UserID = user.UserID, Email = user.Email, Password = user.Password};
            }
            return View(editUserPasswordViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult ChangePassword(EditUserPasswordViewModel eupvm)
        {
            if(ModelState.IsValid) 
            {
                var user = us.GetUsersById(eupvm.UserID);
                if(user != null)
                {
                    us.UpdateUserPassword(eupvm);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("x", "User is not found!");
                    return View(eupvm);
                }
            }
            else
            {
                ModelState.AddModelError("x", "Entered Incorrect Password!");
                return View(eupvm);
            }
        }
    }
}