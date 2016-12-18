using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AKCWeb.Startup))]
namespace AKCWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
