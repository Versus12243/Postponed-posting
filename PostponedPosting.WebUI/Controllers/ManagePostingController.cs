using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.SocialNetworkModels;
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
        [Inject]
        public IRepository<SocialNetwork> SocialNetworkRepository { get; set; }

        // GET: ManagePosting
        public ActionResult Index(int id)
        {
            var sn = SocialNetworkRepository.Find(w => w.Id == id && w.Status == Domain.Entities.StatusEnums.EntityStatus.Active);
            if(sn != null)
            {
                return View(id);
            }
            return View("Error");
        }        
    }
}