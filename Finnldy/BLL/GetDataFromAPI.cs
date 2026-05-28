using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finnldy.BLL
{
    public class GetDataFromAPI
    {
        public string Status;
        public int? Movie_id;
        public string Username;
        public action? Action;

        public enum action
        {
            Liked,
            Disliked,
            Watchlater,
            AlreadyWatched
        }

        public GetDataFromAPI(string Status, int? Movie_id, string Username, action? Action)
        {
            Status = this.Status;
            Movie_id = this.Movie_id;
            Username = this.Username;
            Action = this.Action;
        }
    }
}
