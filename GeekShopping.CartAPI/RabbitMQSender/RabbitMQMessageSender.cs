using GeekShopping.CartAPI.Messages;
using GeekShoppingMessageBus;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace GeekShopping.CartAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }

        public async void SendMessage(BaseMessage Message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                UserName = _userName,
                Password = _password
            };
            _connection = await factory.CreateConnectionAsync();

            using var channel = _connection.CreateChannelAsync().Result;
            await channel.QueueDeclareAsync(queue: queueName, false, false, false, null);

            byte[] body = GetMessageAsByteArray(Message);
            await channel.BasicPublishAsync(
                exchange: "", routingKey: queueName, body: body);
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<CheckoutHeaderVO>((CheckoutHeaderVO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }
    }
}
