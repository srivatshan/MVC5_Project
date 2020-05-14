using MVCProject.Data.Models;
using MVCProject.Data.Repository.Interface;
using MVCProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MVCProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserDetails _userDetails;
        public AccountController(IUserDetails userDetails)
        {
            _userDetails = userDetails;
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = _userDetails.GetUser(loginModel.UserName, loginModel.Password);
                //Validating UserName And Pwd
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid UserName and Password");
                    return View();
                }

                if (loginModel.RememberMe)
                {
                    string myObjectJson = new JavaScriptSerializer().Serialize(user);
                    var cookie = new HttpCookie("UserDetails", myObjectJson)
                    {
                        Expires = DateTime.Now.AddHours(5)
                    };
                    //Store Values in Cookies
                    HttpContext.Response.Cookies.Add(cookie);
                }
                else
                    Session["UserDetails"] = user;

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    Password = registerModel.Password,
                    UserType = "Applicant"
                };
                _userDetails.Add(user);
            }
            return View("Login");
        }
    }
}