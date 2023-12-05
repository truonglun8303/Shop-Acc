using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopAcc.Startup))]
namespace ShopAcc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
