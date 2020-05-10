using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MhmdKoeik_HomeWork3.Startup))]
namespace MhmdKoeik_HomeWork3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
