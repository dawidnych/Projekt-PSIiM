using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LastMinuteWebApp.Startup))]
namespace LastMinuteWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
