using anotaki_api.Models;
using anotaki_api.Queues.Publishers.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace anotaki_api.Queues.Publishers
{
    public class OrderPublisher(IConfiguration config) : IOrderPublisher
    {
        private readonly IConfiguration _config = config;

        public async Task Publish(Order order)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMq:Host"]!,
                Port = int.Parse(_config["RabbitMq:Port"]!),
                UserName = _config["RabbitMq:User"]!,
                Password = _config["RabbitMq:Password"]!
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            // Declara a fila
            string queueName = "order_queue";
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            string message = JsonSerializer.Serialize(order);
            var body = Encoding.UTF8.GetBytes(message);

            var props = new BasicProperties();

            // Publica a mensagem na fila
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, false, basicProperties: props, body: body);

            Console.WriteLine("Pedido publicado na fila.");
        }
    }
}
