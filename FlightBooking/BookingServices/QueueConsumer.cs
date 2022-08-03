using BookingServices.Models;
using Microsoft.Extensions.Hosting;
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

                var body = e.Body;

                var message = Encoding.UTF32.GetString(body);

                flightid = message.Replace(@"\", string.Empty);

                // flightid = Regex.Unescape(message);

                //flightid = (string)JObject.Parse(message);

                //flightid = msg[0].Substring(1);

                //new UpdateController(_serviceProvider).UpdateBookingStatus(flightid);
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
                string airlineid = flightid.Replace(@"\", string.Empty);
                var entity = db.TicketBooking.FirstOrDefault(x => x.AirlineId == airlineid);

                if (entity != null)
                {
                    entity.IsCancelled = 1;
                    db.SaveChanges();
                }
                //return "Updated Succesfully";
            }


        }
    }
}
