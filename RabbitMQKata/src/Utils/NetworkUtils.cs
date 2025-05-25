using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace RabbitMQKata.Utils;

public static class NetworkUtils
{
    public static async Task<bool> IsHostAvailable(string host, int timeoutMs = 3000)
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

    public static async Task<bool> IsTcpPortAvailable(string host, int port, int timeoutMs = 3000)
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