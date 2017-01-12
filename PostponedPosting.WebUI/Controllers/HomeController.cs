using Microsoft.AspNet.Identity.Owin;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.PostModels;
using System.Web;
using System.Web.Mvc;

namespace PostponedPosting.WebUI.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public IRepository<Post> PostRepository { get; set; }

        private ApplicationUserManager _userManager = null;

        public ActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "ManagePosting");
            }
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