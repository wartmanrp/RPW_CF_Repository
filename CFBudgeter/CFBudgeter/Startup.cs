using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CFBudgeter.Startup))]
namespace CFBudgeter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
