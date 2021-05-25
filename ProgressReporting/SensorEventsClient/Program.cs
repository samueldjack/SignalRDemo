using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using ProgressReporting.Models;

namespace SensorEventsClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //TODO: Testing 123
            var connection = new Connection("http://localhost:3669/sensor/events");

            connection.Start().Wait();

            connection.AsObservable<SensorEvent>()
                .GroupBy(e => e.SensorId)
                .CombineLatest()
                .Select(latest => latest.Where(e => e.Reading > 0.75).ToList())
                .Where(latest => latest.Count() >= 2)
                .Subscribe(latest => Console.WriteLine("Sensors {0} show readings greater than 0.75", string.Join(",", latest.Select(e => e.SensorId))));

            Console.Read();

            connection.Stop();
        }
    }
}
