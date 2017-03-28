using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcAspNetIdentityEFSample.Startup))]
namespace MvcAspNetIdentityEFSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
