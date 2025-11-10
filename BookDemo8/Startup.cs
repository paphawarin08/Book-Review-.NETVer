using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BookDemo8.Startup))]
namespace BookDemo8
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
