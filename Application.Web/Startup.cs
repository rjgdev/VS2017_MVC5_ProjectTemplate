using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Application.Web.Startup))]

namespace Application.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}