using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PostponedPosting.WebUI.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/Credentials")]
    public class CredentialsController : ApiController
    {
        [Inject]
        public IUserService UserService { get; set; }

        [HttpPost]
        [Route("GetUserCredentials/{id}")]
        public IHttpActionResult GetUserCredentials([FromUri]int id)
        {
            try
            {                
                var credentials = UserService.GetCredentials(id, User.Identity.GetUserId());
                return Ok(credentials);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SaveCredentials")]
        public IHttpActionResult SaveCredentials([FromBody]CredentialsViewModel model)
        {
            try
            {
               var result = UserService.SaveCredentials(model, User.Identity.GetUserId());
               return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}