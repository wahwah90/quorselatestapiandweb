using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Quorse.Web.Startup))]
namespace Quorse.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
