using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostponedPosting.WebUI.Controllers
{
    [Authorize]
    public class SocialNetworksController : Controller
    {
        [Inject]
        public ISocialNetworksService SocialNetworkService { get; set; }

        // GET: SocialNetworks
        public ActionResult Index()
        {
            var model = SocialNetworkService.GetAviableSNForUser(User.Identity.GetUserId());
            return View(model);
        }

        public ActionResult GetListOfSNsForPostingMenu()
        {
            var model = SocialNetworkService.GetAviableSNForUser(User.Identity.GetUserId());
            return PartialView(model);
        }
    }
}