using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace Finnldy.DAL.Database
{
    public class ClientNetworkService
    {
        private TcpClient? client;
        private StreamReader? reader;
        private StreamWriter? writer;

        private bool isConnected;

        public event Action<NetworkPacket>? PacketReceived;
        public event Action<string>? StatusChanged;

        // Chat Verwendung einzelene Sachen:
        // Invoke Diem hat es mir dann erklärt :)
        // ReadLineAsync hat mir Chat auch empfohlen

        // Solche Lines hab ich als CL angegeben 

        public async Task ConnectAsync(string hostIp)
        {
            if (isConnected)
            {
                return;

            }

            try
            {
                client = new TcpClient();

                await client.ConnectAsync(hostIp, 5000);

                // KI anfang
                // CHat
                // Kannst du mir bei dieser klasse helfen irgedwiw skipped es alles manchmal

                NetworkStream stream = client.GetStream(); 

                reader = new StreamReader(stream, Encoding.UTF8);
                

                writer = new StreamWriter(stream, Encoding.UTF8)
                {
                    AutoFlush = true

                };

                isConnected = true;

                StatusChanged?.Invoke($"Verbunden mit Host {hostIp}:{5000}");

                _ = Task.Run(ListenAsync);

                // KI Ende
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

                    string? json = await reader.ReadLineAsync(); //CL



                    if (string.IsNullOrWhiteSpace(json))
                    {
                        break;

                    }

                    NetworkPacket? packet = JsonSerializer.Deserialize<NetworkPacket>(json);


                    if (packet == null)
                    {
                        continue;

                    }

                    PacketReceived?.Invoke(packet); //CL
                }
            }
            catch
            {
                //StatusChanged?.Invoke("Verbindung getrennt.");

                if (StatusChanged == null)
                {
                    StatusChanged("verbinddung getrennt.");
                }
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
                StatusChanged?.Invoke("Nicht verbunden nachricht wurde nicht gesendet."); //CL
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