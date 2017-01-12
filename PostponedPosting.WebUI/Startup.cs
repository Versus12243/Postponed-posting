using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PostponedPosting.WebUI.Startup))]
namespace PostponedPosting.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
