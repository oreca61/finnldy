namespace Finnldy.BLL
{
    public class GetDataFromAPI
    {
        public string Status { get; set; } = "";
        public int? Movie_id { get; set; }
        public string Username { get; set; } = "";
        public action? Action { get; set; }

        public enum action
        {
            Liked,
            Disliked,
            Watchlater,
            AlreadyWatched
        }

        public GetDataFromAPI()
        {
        }

        public GetDataFromAPI(string status, int? movieId, string username, action? action)
        {
            Status = status;
            Movie_id = movieId;
            Username = username;
            Action = action;
        }
    }
}