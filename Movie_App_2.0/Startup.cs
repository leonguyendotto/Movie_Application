using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Movie_App_2._0.Startup))]
namespace Movie_App_2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
