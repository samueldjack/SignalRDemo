using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using ProgressReporting.Models;

namespace ProgressReporting.Hubs
{
    public class SensorEventsConnection : PersistentConnection
    {
        private static int _connectionCount;
        private static IPersistentConnectionContext _context;
        private static IDisposable _subscription;

        protected override Task OnConnected(IRequest request, string connectionId)
        {
            var currentValue = Interlocked.Increment(ref _connectionCount);
            if (currentValue == 1)
            {
                StartSendingEvents();
            }

            return base.OnConnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId)
        {
            var currentValue = Interlocked.Decrement(ref _connectionCount);
            if (currentValue == 0)
            {
                StopSendingEvents();
            }

            return base.OnDisconnected(request, connectionId);
        }

        private static void StartSendingEvents()
        {
            var random = new Random();
            if (_context == null)
            {
                _context = GlobalHost.ConnectionManager.GetConnectionContext<SensorEventsConnection>();
            }

            _subscription = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(50))
                .Select(_ => new SensorEvent() { Reading = random.NextDouble(), SensorId = random.Next(1,4)})
                .Subscribe(sensorEvent => _context.Connection.Broadcast(sensorEvent));
        }

        private static void StopSendingEvents()
        {
            _subscription.Dispose();
        }
    }
}