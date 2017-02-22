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
    public class LinksController : Controller
    {
        [Inject]
        public IGroupsOfLinksService GroupsOfLinksService { get; set; }
        [Inject]
        public ILinksService LinksService { get; set; }

        // GET: Links
        public ActionResult Index(int id)
        {
            var group = GroupsOfLinksService.GetGroupById(id, User.Identity.GetUserId());
            if(group == null)
            {
                return View("Error");
            }

            ViewBag.GroupId = group.Id;
            ViewBag.GroupName = group.Name;

            return View();

            //try
            //{
            //   // var linksForGroup = LinksService.GetAllLinksForGroup(User.Identity.GetUserId(), id, showAll);

            //  //  return View(linksForGroup);
            //}
            //catch(Exception ex)
            //{
            //    return View("Error");
            //}    
        }
    }
}