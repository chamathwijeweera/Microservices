using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConnection connecion;
        private readonly IModel channel;

        public MessageBusClient(IConfiguration configuration)
        {
            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"])
            };
            try
            {
                connecion = factory.CreateConnection();
                channel = connecion.CreateModel();
                channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
                connecion.ConnectionShutdown += RabbitMQConnectionShutdown;
                Console.WriteLine("--> Connected to message bus ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not connect to the message bus, error: {ex.Message}");
            }
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (connecion.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ connection open, sending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connection closed, not sending");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish("trigger", string.Empty, null, body);
            Console.WriteLine("--> Message sent");
        }

        private void RabbitMQConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            Console.WriteLine("--> RabbitMQ connection shutdown");
        }
    }
}
