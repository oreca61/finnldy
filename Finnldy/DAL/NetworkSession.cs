using Finnldy.DAL.Database;

namespace Finnldy.DAL
{
    public static class NetworkSession
    {
        public static HostNetworkService Host { get; } = new HostNetworkService();
        public static ClientNetworkService Client { get; } = new ClientNetworkService();

        public static LobbyController LobbyController { get; } = new LobbyController();

        public static bool IsHost { get; set; }
        public static bool IsClient { get; set; }

        public static string Username { get; set; } = Environment.MachineName;
    }
}