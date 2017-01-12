using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostponedPosting.WebUI.Controllers
{
    //[Authorize]
    public class ManagePostingController : Controller
    {
        // GET: ManagePosting
        public ActionResult Index()
        {
            return View();
        }        
    }
}