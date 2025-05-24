using EasyNetQ;

namespace RabbitMQKata.Publisher;

public class MessagePublisher
{
    public async Task foo()
    {
        // Connection string:
        // For local RabbitMQ (not in Docker compose yet): "host=localhost;username=guest;password=guest"
        // When using Docker Compose later, it will be: "host=rabbitmq-easynetq;username=guest;password=guest"
        // where 'rabbitmq-easynetq' is the service name of RabbitMQ in docker-compose.yml
        var connectionString = "host=localhost;username=_;password=_";

        // For now, let's assume the C# app is NOT in Docker and RabbitMQ IS.
        // If your C# app IS in Docker, and RabbitMQ is ALSO in Docker (but not via compose yet),
        // You might need to use host.docker.internal (Windows/Mac) or the Docker bridge IP.
        // We'll fix this properly with Docker Compose in Phase 4.

        Console.WriteLine("Attempting to connect to RabbitMQ...");
        Console.WriteLine($"Connection string: {connectionString}");

        try
        {
            // The IBus is the main entry point to EasyNetQ. It is IDisposable.
            using (var bus = RabbitHutch.CreateBus(connectionString))
            {
                Console.WriteLine("Connected to RabbitMQ!");

                for (int messageId = 1; messageId <= 10; messageId++)
                {
                    var message = new TextMessage(messageId, $"Product \"{messageId}\"");
                    
                    // Publish the message
                    // EasyNetQ will create an exchange based on the message type if it doesn't exist.
                    // Exchange name convention: Namespace.ClassName:Version (e.g., MyMessages.TextMessage:1)
                    await bus.PubSub.PublishAsync(message);

                    Console.WriteLine($"Sent: {message}");
                }
            } // bus is disposed of here, closing the connection.
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
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }

        Console.WriteLine("Publisher shut down.");
        Thread.Sleep(10_000);
    }
}