using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NZNA.Startup))]
namespace NZNA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
