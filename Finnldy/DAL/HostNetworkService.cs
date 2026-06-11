using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Finnldy.DAL
{
    public class HostNetworkService
    {
        private TcpListener? listener;
        private readonly List<TcpClient> clients = new List<TcpClient>();
        private bool isRunning;

        public event Action<NetworkPacket>? PacketReceived;
        public event Action<string>? StatusChanged;

        public async Task StartAsync(int port = 5000)
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;

            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            StatusChanged?.Invoke($"Host gestartet. IP: {GetLocalIpAddress()} Port: {port}");

            while (isRunning)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();

                    lock (clients)
                    {
                        clients.Add(client);
                    }

                    StatusChanged?.Invoke("Client verbunden.");

                    _ = Task.Run(() => HandleClientAsync(client));
                }
                catch
                {
                    if (isRunning)
                    {
                        StatusChanged?.Invoke("Fehler beim Verbinden eines Clients.");
                    }
                }
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                while (isRunning && client.Connected)
                {
                    string? json = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        break;
                    }

                    NetworkPacket? packet = JsonSerializer.Deserialize<NetworkPacket>(json);

                    if (packet == null)
                    {
                        continue;
                    }

                    PacketReceived?.Invoke(packet);

                    await SendToAllExceptAsync(packet, client);
                }
            }
            catch
            {
                StatusChanged?.Invoke("Client getrennt.");
            }
            finally
            {
                lock (clients)
                {
                    clients.Remove(client);
                }

                client.Close();
            }
        }

        public async Task SendToAllAsync(NetworkPacket packet)
        {
            string json = JsonSerializer.Serialize(packet) + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            List<TcpClient> clientsCopy;

            lock (clients)
            {
                clientsCopy = clients.ToList();
            }

            foreach (TcpClient client in clientsCopy)
            {
                try
                {
                    if (!client.Connected)
                    {
                        continue;
                    }

                    NetworkStream stream = client.GetStream();
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                    await stream.FlushAsync();
                }
                catch
                {
                    lock (clients)
                    {
                        clients.Remove(client);
                    }

                    client.Close();
                }
            }
        }

        private async Task SendToAllExceptAsync(NetworkPacket packet, TcpClient exceptClient)
        {
            string json = JsonSerializer.Serialize(packet) + "\n";
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            List<TcpClient> clientsCopy;

            lock (clients)
            {
                clientsCopy = clients.ToList();
            }

            foreach (TcpClient client in clientsCopy)
            {
                if (client == exceptClient)
                {
                    continue;
                }

                try
                {
                    if (!client.Connected)
                    {
                        continue;
                    }

                    NetworkStream stream = client.GetStream();
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                    await stream.FlushAsync();
                }
                catch
                {
                    lock (clients)
                    {
                        clients.Remove(client);
                    }

                    client.Close();
                }
            }
        }

        public void Stop()
        {
            isRunning = false;

            lock (clients)
            {
                foreach (TcpClient client in clients)
                {
                    client.Close();
                }

                clients.Clear();
            }

            listener?.Stop();

            StatusChanged?.Invoke("Host gestoppt.");
        }

        public static string GetLocalIpAddress()
        {
            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
    }
}