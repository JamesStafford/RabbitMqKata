using RabbitMQKata.Publisher;
using RabbitMQKata.Subscriber;
using RabbitMQKata.Utils;

string? rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
Console.WriteLine($"rabbitUser: \"{rabbitUser}\"");

string? rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
Console.WriteLine($"rabbitPassword: \"{rabbitUser}\"");

if (string.IsNullOrWhiteSpace(rabbitUser) || string.IsNullOrWhiteSpace(rabbitPassword))
{
    throw new ApplicationException("Missing environment variables RABBITMQ_USER and RABBITMQ_PASSWORD.");
}

if (string.IsNullOrWhiteSpace(rabbitUser))
{
    throw new ApplicationException("Missing environment variables RABBITMQ_USER.");
}

if (string.IsNullOrWhiteSpace(rabbitPassword))
{
    throw new ApplicationException("Missing environment variables RABBITMQ_PASSWORD.");
}

string rabbitHost = "rabbit";

Task<bool> isRabbitHostAvailableTask = NetworkUtils.IsHostAvailable(rabbitHost);
isRabbitHostAvailableTask.Wait();
bool isRabbitHostAvailable = isRabbitHostAvailableTask.Result;

Console.WriteLine($"isRabbitHostAvailable: \"{isRabbitHostAvailable}\"");

if (isRabbitHostAvailable)
{
    Task<bool> isPort5672AvailableTask = NetworkUtils.IsTcpPortAvailable(rabbitHost, 5672);
    isPort5672AvailableTask.Wait();
    bool isPort5672Available = isPort5672AvailableTask.Result;
    Console.WriteLine($"isPort5672Available: \"{isPort5672Available}\"");
    
    Task<bool> isPort15672AvailableTask = NetworkUtils.IsTcpPortAvailable(rabbitHost, 15672);
    isPort15672AvailableTask.Wait();
    bool isPort15672Available = isPort15672AvailableTask.Result;
    Console.WriteLine($"isPort15672Available: \"{isPort15672Available}\"");
}

// Create a cancellation token source for coordinated shutdown
using var cts = new CancellationTokenSource();

try
{
    // Start subscriber task
    var subscriberTask = Task.Run(() => MessageSubscriber.SubscribeToMessages(rabbitUser, rabbitPassword), cts.Token);

    // Give the subscriber a moment to set up
    await Task.Delay(2000, cts.Token); // Wait 2 seconds

    // Start publisher task
    var publisherTask = Task.Run(() => MessagePublisher.WriteTestMessages(rabbitUser, rabbitPassword), cts.Token);

    // Wait for both tasks to complete or until cancellation is requested
    await Task.WhenAll(publisherTask, subscriberTask);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    cts.Cancel(); // Ensure we cancel any remaining operations
}

await Task.Delay(60*1000, cts.Token);
