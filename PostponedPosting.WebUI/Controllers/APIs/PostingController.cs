using Microsoft.AspNet.Identity;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace PostponedPosting.WebUI.Controllers.APIs
{
    [Authorize]
    [RoutePrefix("api/Posting")]
    public class PostingController : ApiController
    {
        [Inject]
        public IPostingService PostingService { get; set; }

        [HttpPost]
        [Route("PostsDataHandler")]
        public JsonResult<object> PostsDataHandler(PostDTViewModel param)
        {
            try
            {
                return Json((object)PostingService.PostsDataHandler(param, User.Identity.GetUserId()));
            }
            catch (Exception ex)
            {
                return Json((object)(new { error = ex.Message }));
            }
        }

        [HttpPost]
        [Route("SelectetGroupsDataHandler")]
        public JsonResult<object> SelectetGroupsDataHandler(PostSelectedGroupDTViewModel param)
        {
            try
            {
                return Json((object)PostingService.SelectetGroupsDataHandler(param, User.Identity.GetUserId()));
            }
            catch (Exception ex)
            {
                return Json((object)(new { error = ex.Message }));
            }
        }

        [HttpPost]
        [Route("EditPost")]
        public async Task<IHttpActionResult> EditPost(PostViewModel model)
        {
            try
            {
                int modelId = await PostingService.EditPostAsync(model, User.Identity.GetUserId());

                return Ok(modelId);
            }
            catch(Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("GetPostData/{postId}")]
        public IHttpActionResult GetPostData([FromUri]int postId)
        {
            try
            {
                var result = PostingService.GetPostData(User.Identity.GetUserId(), postId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("SwitchSendingStatus/{postId}")]
        public IHttpActionResult SwitchSendingStatus(int postId)
        {
            try
            {
                var result = PostingService.SwitchSendingStatus(User.Identity.GetUserId(), postId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}