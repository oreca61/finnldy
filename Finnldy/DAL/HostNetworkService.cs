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

        private readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        // Chat Verwendung einzelene Sachen:
        // Invoke Diem hat es mir dann erklärt :)
        // lock dass nur ein thread auf einmal geht
        // ReadLineAsync hat mir Chat auch empfohlen
        // und Encoding.UTF8.GetBytes(json);

        // Solche Lines hab ich als CL angegeben 

        public async Task StartAsync()
        {
            if (isRunning)
            {
                return;
            }

            isRunning = true;

            listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();

            StatusChanged?.Invoke($"Host gestartet. IP: {GetLocalIpAddress()} Port: {5000}");

            // KI 
            // Kannst du meine Schleife verbessern da müsste ein Fehler drinnen sein
            while (isRunning)
            {
                try
                {


                    TcpClient client = await listener.AcceptTcpClientAsync();

                    lock (clients)
                    {
                        clients.Add(client);

                    }

                    // Davor habe ich invoke verwenden aber hat irgedwie nicht funktioniert

                    if (StatusChanged != null)
                    {
                        StatusChanged("Client verbunden.");

                    }



                    _ = Task.Run(() => HandleClientAsync(client));
                }
            // KI ende
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

                    string? json = await reader.ReadLineAsync(); //CL

                    if (string.IsNullOrWhiteSpace(json))
                    {

                        break;
                    }

                    NetworkPacket? packet = JsonSerializer.Deserialize<NetworkPacket>(json, jsonOptions);


                    if (packet == null)
                    {
                    
                        continue;
                    }

                    PacketReceived?.Invoke(packet); // CL

                    await SendToAllExceptAsync(packet, client);
                }
            }
            catch
            {
                if (StatusChanged == null)
                {
                    StatusChanged("Client getrennt.");
                }
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

            byte[] bytes = Encoding.UTF8.GetBytes(json); //CL

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

            byte[] bytes = Encoding.UTF8.GetBytes(json); //CL

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
                    await stream.WriteAsync(bytes, 0, bytes.Length); //CL

                    await stream.FlushAsync(); // CL
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

            StatusChanged?.Invoke("Host gestoppt."); // CL
        }

        public static string GetLocalIpAddress()
        {

            foreach (IPAddress ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList) // Diese If bedngung hat Chat gemacht habe ihn den prottypen vom if gesschickt und mir das gegeben
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