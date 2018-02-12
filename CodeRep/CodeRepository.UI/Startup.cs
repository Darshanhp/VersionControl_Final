using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CodeRepository.UI.Startup))]
namespace CodeRepository.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
