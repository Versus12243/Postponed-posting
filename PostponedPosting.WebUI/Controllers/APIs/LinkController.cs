using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModel;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;

namespace PostponedPosting.WebUI.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/Links")]
    public class LinkController : ApiController
    {
        [Inject]
        public ILinksService LinksService { get; set; }

        [HttpPost]
        [Route("DataHandler")]
        public JsonResult<object> DataHandler(LinkDTViewModel param)
        {
            try
            {
                return Json((object)LinksService.DataHandler(param, User.Identity.GetUserId(), param.GroupId, param.ShowAll));
            }
            catch(Exception ex)
            {
                return Json((object)(new { error = ex.Message }));
            }

        }

        [HttpPost]
        [Route("EditLink")]
        public IHttpActionResult EditLink(LinkViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = LinksService.EditLink(model, User.Identity.GetUserId());
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Both fields are required");
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SwitchLinkPresenceInGroup/{linkId}/{groupId}")]
        public IHttpActionResult SwitchLinkPresenceInGroup([FromUri]int linkId, [FromUri]int groupId)
        {
            try
            {
                var result = LinksService.SwitchLinkPresenceInGroup(User.Identity.GetUserId(), linkId, groupId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}