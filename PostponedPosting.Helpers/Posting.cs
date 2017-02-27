using Facebook;
using System;

namespace PostponedPosting.Helpers
{
    public class Posting
    {
        public void Facebook(string access_token)
        {
            string postUrl = "https://graph.facebook.com/100009148912073/feed";           

            //postParameters = string.Format("message={0}&access_token={1}",
            //                                    "asdfsafdwe",
            //                                    "190008478137131|VtSP8Ncrz1oVdlG1JRKXG3qiUcQ");
            try
            {
               //MyWebRequest req = new MyWebRequest(postUrl);
               // string response = req.GetResponse();
                FacebookClient client = new FacebookClient();
                client.AppId = "190008478137131";
                client.AppSecret = "818f4255caa3084c3ea52cf5a5f16505";
                client.AccessToken = "VtSP8Ncrz1oVdlG1JRKXG3qiUcQ";
                var s = client.Post(postUrl);
            }
            catch (Exception ex)
            {
                throw new Exception("An error ocurred while posting data to the user's wall: " + postUrl + "?", ex);
            }
        }
    }
}
