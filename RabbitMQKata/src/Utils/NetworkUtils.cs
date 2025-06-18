using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RabbitMQKata.Utils;

public static class NetworkUtils
{
    public static void ValidateRabbitMqConfiguration(string? rabbitUser, string? rabbitPassword)
    {
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
    }
    
    private static async Task<bool> IsHostAvailable(string host, int timeoutMs = 3000)
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(host, timeoutMs);
            return reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
    }

    private static async Task<bool> IsTcpPortAvailable(string host, int port, int timeoutMs = 3000)
    {
        try
        {
            using var client = new TcpClient();
            var connectTask = client.ConnectAsync(host, port);
            
            using var cts = new CancellationTokenSource(timeoutMs);
            await using (cts.Token.Register(() => client.Close()))
            {
                await connectTask;
                return true;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }

    
}