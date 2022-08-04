using BookingServices.Models;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;


namespace BookingServices
{
    public class QueueConsumer : IHostedService
    {

        public string flightid;
        private readonly IModel channel;
        private readonly IConnectionFactory _factory;
        private readonly IServiceProvider _serviceProvider;

        public QueueConsumer(IServiceProvider serviceProvider)
        {
            _factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };
            _serviceProvider = serviceProvider;
            channel = _factory.CreateConnection().CreateModel();
        }


        public void Consume()
        {
            //string[] msg;

            channel.QueueDeclare("Test-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (sender, e) =>
            {

                var body = e.Body.ToArray();
                var message = Encoding.UTF32.GetString(body);
                flightid = (string)JsonConvert.DeserializeObject(message);

                UpdateBookingStatus(flightid);
            };

            channel.BasicConsume("Test-queue", true, consumer);

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Consume();
            return Task.CompletedTask;
        }

        public void UpdateBookingStatus(string flightid)
        {
            using (var db = new UserDBContext())
            {
                
                var entity = db.TicketBooking.Where(x => x.AirlineId == flightid).ToList();

                if (entity != null)
                {
                    foreach(var row in entity)
                    {
                        row.IsCancelled = 1;
                        db.SaveChanges();
                    }
                   
                }
                
            }


        }
    }
}
