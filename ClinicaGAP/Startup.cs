using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ClinicaGAP.Startup))]
namespace ClinicaGAP
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
