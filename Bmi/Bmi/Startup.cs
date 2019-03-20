using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Bmi.Startup))]
namespace Bmi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
        }
    }
}