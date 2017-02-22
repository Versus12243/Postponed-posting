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
    public class GroupsOfLinksController : Controller
    {
        [Inject]
        public IGroupsOfLinksService GroupsOfLinksService { get; set; }

        // GET: PagesGroups
        public ActionResult Index(int id)
        {
            var groups = GroupsOfLinksService.GetAllGroupsOfLinks(User.Identity.GetUserId(), id);

            return View(groups);
        }        
    }
}