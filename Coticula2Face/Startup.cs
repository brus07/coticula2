using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Coticula2Face.Startup))]
namespace Coticula2Face
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
