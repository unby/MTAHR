using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebMTHR.Startup))]
namespace WebMTHR
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
