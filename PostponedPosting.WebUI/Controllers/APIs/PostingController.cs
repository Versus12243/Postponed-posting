using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
using Ninject;
using PostponedPosting.Domain.Core;
using PostponedPosting.Domain.Entities.PostModels;
using PostponedPosting.Persistence.ApplicationService.Abstract;
using PostponedPosting.Persistence.ServiceModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
                return BadRequest(ex.Message);
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

        [HttpDelete]
        public IHttpActionResult DeletePost(int id)
        {
            try
            {
                var result = PostingService.DeletePost(User.Identity.GetUserId(), id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("ShortedLink")]
        public async Task<IHttpActionResult> ShortedLink(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    url = HttpUtility.UrlDecode(url);
                    var responseString = await client.GetStringAsync("https://api-ssl.bitly.com/v3/shorten?" + "access_token=dd1b0720edb44e71d43a9923c9ebbc397baff27a&longUrl=" + url);                
                    return Ok(JObject.Parse(responseString));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetDebugInfoAboutSharedLink")]
        public async Task<IHttpActionResult> GetDebugInfoAboutSharedLink(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    url = HttpUtility.UrlDecode(url);
                    var values = new Dictionary<string, string>
                    {
                        { "debug", "warning" },
                        { "format", "json" },
                        { "id", url },
                        { "method", "post"},
                        { "pretty", "0" },
                        { "scrape", "true" },
                        { "suppress_http_code", "1" }
                    };

                    var content = new FormUrlEncodedContent(values);

                    var response = await client.PostAsync("https://graph.facebook.com/v2.8/?access_token=190008478137131|818f4255caa3084c3ea52cf5a5f16505", content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    return Ok(JObject.Parse(responseString));
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}