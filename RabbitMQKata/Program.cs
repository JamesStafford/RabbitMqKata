using RabbitMQKata.Utils;

string? rabbitUser = Environment.GetEnvironmentVariable("RABBITMQ_USER");
Console.WriteLine($"rabbitUser: \"{rabbitUser}\"");

string? rabbitPassword = Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD");
Console.WriteLine($"rabbitPassword: \"{rabbitPassword}\"");

NetworkUtils.ValidateRabbitMqConfiguration(rabbitUser, rabbitPassword);
