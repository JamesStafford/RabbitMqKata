using EasyNetQ;
using RabbitMQKata.Publisher;

namespace RabbitMQKata.Subscriber;

public static class MessageSubscriber
{
    public static async Task SubscribeToMessages(string username, string password)
    {
        var connectionString = $"host=rabbit;username={username};password={password}";
    
        using var bus = RabbitHutch.CreateBus(connectionString);

        while (true)
        {
            await bus.PubSub.SubscribeAsync<TextMessage>(
                subscriptionId: "my_subscription", // unique subscription ID
                message =>
                {
                    Console.WriteLine($"Received message: {message.Content}");
                    return Task.CompletedTask;
                });
            
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}