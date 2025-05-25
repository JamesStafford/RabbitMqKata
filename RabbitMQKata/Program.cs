using RabbitMQKata.Publisher;

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

// MessagePublisher.WriteTestMessages().Wait();