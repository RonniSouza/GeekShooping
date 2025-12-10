namespace GeekShoppingMessageBus
{
    public interface IMessageBus
    {
        Task PublishMessage<T>(BaseMessage  message, string queueName);
    }
}
