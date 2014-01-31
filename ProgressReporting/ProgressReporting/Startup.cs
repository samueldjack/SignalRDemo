using Microsoft.Owin;
using Owin;
using ProgressReporting.Hubs;

[assembly: OwinStartupAttribute(typeof(ProgressReporting.Startup))]
namespace ProgressReporting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<SensorEventsConnection>("/sensor/events");
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
