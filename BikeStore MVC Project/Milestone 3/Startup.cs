using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Milestone_3.Startup))]
namespace Milestone_3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
