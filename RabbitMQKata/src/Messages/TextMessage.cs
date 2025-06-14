namespace RabbitMQKata.Publisher
{
    public class TextMessage
    {
        public TextMessage()
        {
        }

        public TextMessage(int messageId, string content)
        {
            MessageId = messageId;
            Content = content;
        }

        public TextMessage(int messageId, string content, DateTime timestamp)
        {
            MessageId = messageId;
            Content = content;
            Timestamp = timestamp;
        }

        public int MessageId { get; init; } = 0;
        public string Content { get; init; } = string.Empty;
        public DateTime Timestamp { get; init; } = DateTime.Now;

        public override string ToString()
        {
            return $"ID: {MessageId}, Content: '{Content}', Sent: {Timestamp}";
        }
    }
}