using Microsoft.AspNet.Identity.Owin;
using PostponedPosting.Domain.Core;
using PostponedPosting.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Postponed_posting.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager _userManager = null;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult post()
        {
            var posting = new Posting();
            var access_token = UserManager.Users.First().Logins.First().ProviderKey;

            posting.Facebook(access_token);
            return RedirectToAction("Home");
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }
}