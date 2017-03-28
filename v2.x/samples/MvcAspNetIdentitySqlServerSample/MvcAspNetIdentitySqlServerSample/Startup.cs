using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcAspNetIdentitySqlServerSample.Startup))]
namespace MvcAspNetIdentitySqlServerSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
