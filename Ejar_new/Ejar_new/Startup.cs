using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Ejar_new.Startup))]
namespace Ejar_new
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
