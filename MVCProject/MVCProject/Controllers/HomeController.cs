using MVCProject.Data.Models;
using MVCProject.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCProject.Controllers
{
    [AuthendicationFilter]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Getting User Details from Cookies or session
            User UserDetails = (User)Session["UserDetails"] ?? JsonConvert.DeserializeObject<User>(Request.Cookies["UserDetails"].Value);
            if (UserDetails.UserType == "Admin")
            {
                ViewBag.SearchEnable = true;
            }

            return View();
        }
    }
}