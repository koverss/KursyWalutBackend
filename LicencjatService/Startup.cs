using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(KursyWalutService.Startup))]

namespace KursyWalutService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}