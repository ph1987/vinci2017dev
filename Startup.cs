using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(vinci_novo.Startup))]
namespace vinci_novo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
