using RabbitMQKata.Publisher;
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

string rabbitHost = "rabbitmq";

Task<bool> isRabbitHostAvailableTask = NetworkUtils.IsHostAvailable(rabbitHost);
isRabbitHostAvailableTask.Wait();
bool isRabbitHostAvailable = isRabbitHostAvailableTask.Result;

Console.WriteLine($"isRabbitHostAvailable: \"{isRabbitHostAvailableTask}\"");

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


// MessagePublisher.WriteTestMessages(rabbitUser, rabbitPassword).Wait();