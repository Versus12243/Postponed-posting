using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Postponed_posting.Startup))]
namespace Postponed_posting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
