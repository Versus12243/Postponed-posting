using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.RequestModel;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace PostponedPosting.WebUI.Controllers.APIs
{
    [Authorize]
    public class PostingController : ApiController
    {
        [Inject]
        public IPostingService PostingService { get; set; }

        [HttpPut]
        public async Task<IHttpActionResult> PutPostToScheduler(PostViewModel id)
        {
            try
            {
                int modelId = await PostingService.AddPostAsync(id, User.Identity.GetUserId());

                return Ok(modelId);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}