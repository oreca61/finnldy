using Finnldy.BLL;

namespace Finnldy.DAL
{
    public class NetworkPacket
    {
        public string Type { get; set; } = "";
        public string Username { get; set; } = "";
        public int MovieId { get; set; }
        public string MovieTitle { get; set; } = "";
        public SwipeType SwipeType { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}