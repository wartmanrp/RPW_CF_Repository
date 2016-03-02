using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugSquish.Startup))]
namespace BugSquish
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
