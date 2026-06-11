using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Finnldy.DAL
{
    public class ClientNetworkService
    {
        private TcpClient? client;
        private StreamReader? reader;
        private StreamWriter? writer;
        private bool isConnected;

        public event Action<NetworkPacket>? PacketReceived;
        public event Action<string>? StatusChanged;

        public async Task ConnectAsync(string hostIp, int port = 5000)
        {
            if (isConnected)
            {
                return;
            }

            try
            {
                client = new TcpClient();
                await client.ConnectAsync(hostIp, port);

                NetworkStream stream = client.GetStream();

                reader = new StreamReader(stream, Encoding.UTF8);
                writer = new StreamWriter(stream, Encoding.UTF8)
                {
                    AutoFlush = true
                };

                isConnected = true;

                StatusChanged?.Invoke($"Verbunden mit Host {hostIp}:{port}");

                _ = Task.Run(ListenAsync);
            }
            catch (Exception ex)
            {
                StatusChanged?.Invoke("Verbindung fehlgeschlagen: " + ex.Message);
            }
        }

        private async Task ListenAsync()
        {
            try
            {
                while (isConnected && reader != null)
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
                }
            }
            catch
            {
                StatusChanged?.Invoke("Verbindung zum Host verloren.");
            }
            finally
            {
                Disconnect();
            }
        }

        public async Task SendAsync(NetworkPacket packet)
        {
            if (!isConnected || writer == null)
            {
                StatusChanged?.Invoke("Nicht verbunden. Nachricht wurde nicht gesendet.");
                return;
            }

            string json = JsonSerializer.Serialize(packet);

            await writer.WriteLineAsync(json);
        }

        public void Disconnect()
        {
            isConnected = false;

            reader?.Close();
            writer?.Close();
            client?.Close();

            StatusChanged?.Invoke("Client getrennt.");
        }
    }
}