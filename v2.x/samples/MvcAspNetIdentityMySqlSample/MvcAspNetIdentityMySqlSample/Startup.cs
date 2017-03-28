using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcAspNetIdentityMySqlSample.Startup))]
namespace MvcAspNetIdentityMySqlSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
