using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PostponedPosting.WebUI.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/GroupsOfLinks")]
    public class GroupsOfLinksController : ApiController
    {
        [Inject]
        public IGroupsOfLinksService GroupsOfLinksService { get; set; }

        // GET: api/PagesGroup
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("EditGroupOfLinks/{socialNetworkId}")]
        public IHttpActionResult EditGroupOfLinks([FromUri]int socialNetworkId, GroupOfLinksViewModel model)
        {
            try
            {
                var result = GroupsOfLinksService.EditGroupOfLinks(User.Identity.GetUserId(), socialNetworkId, model);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("GetAllLinksOfGroup/{groupId}")]
        public IHttpActionResult GetAllLinksOfGroup([FromUri]int groupId)
        {
            try
            {
                var links = GroupsOfLinksService.GetAllLinksOfGroup(User.Identity.GetUserId(), groupId);
                return Ok(links);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/PagesGroup/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/PagesGroup
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PagesGroup/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PagesGroup/5
        public void Delete(int id)
        {
        }
    }
}
