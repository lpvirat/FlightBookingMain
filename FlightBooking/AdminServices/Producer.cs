
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace AdminServices
{
    public class Producer
    {
        public static void Publish(IModel channel,string flightId)
        {
            channel.QueueDeclare("Test-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
                );
            var count = 0;
            var message = flightId;
            var body = Encoding.UTF32.GetBytes(JsonConvert.SerializeObject(message));
            channel.BasicPublish("", "Test-queue", null, body);
            count++;
            Thread.Sleep(1000);

        }
    }
}
