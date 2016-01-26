using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CFblog.Startup))]
namespace CFblog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
