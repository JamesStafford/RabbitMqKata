using EasyNetQ;
using EasyNetQ.DI;

namespace RabbitMQKata.Publisher;

public static class MessagePublisher
{
    public static async Task WriteTestMessages(string username, string password)
    {
        var connectionString = $"host=rabbit;username={username};password={password}";
        
        Console.WriteLine($"Connection string: {connectionString}");

        while (true)
        {
            using var bus = RabbitHutch.CreateBus(connectionString);

            try
            {
                for (int messageId = 1; messageId <= 10; messageId++)
                {
                    var text = $"Product \"{messageId}\"";
                    var message = new TextMessage(messageId, text);

                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5*60));
                    try
                    {
                        Console.WriteLine($"Sending: \"{text}\"");
                        await bus.PubSub.PublishAsync(
                            message,
                            cancellationToken: cts.Token);
                        Console.WriteLine($"Sent: \"{text}\"");
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine($"Publishing message \"{messageId}\" timed out or was cancelled");
                        throw;
                    }

                    Console.WriteLine($"Sent: \"{message}\"");
                }
            }
            catch (EasyNetQ.EasyNetQException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"EasyNetQ connection error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                Console.ResetColor();
                Console.WriteLine("Is RabbitMQ running and accessible on localhost:5672?");
                Console.WriteLine("If C# app is in Docker, ensure network config allows access to RabbitMQ.");
            }
            catch (OperationCanceledException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Operation timed out while trying to publish message");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("Publisher shut down.");

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}