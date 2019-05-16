namespace Cynosura.Messaging
{
    public class RabbitMessagingServiceOptions
    {
        public string ConnectionUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string BrokerX509Cn { get; set; }
    }
}