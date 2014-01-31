using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ProgressReporting.Hubs
{
    public class PageViewersHub : Hub
    {
        public static int _viewerCount;

        public void ViewerCountChanged(int viewerCount)
        {
            Clients.All.viewerCountChanged(viewerCount);
        }

        public override Task OnConnected()
        {
            Interlocked.Increment(ref _viewerCount);
            ViewerCountChanged(_viewerCount);

            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            Interlocked.Decrement(ref _viewerCount);
            ViewerCountChanged(_viewerCount);

            return base.OnDisconnected();
        }
    }
}