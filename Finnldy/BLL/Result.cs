using System.Collections.Generic;

namespace Finnldy.BLL
{
    public class Result
    {
        public Movies Movie { get; private set; }

        private HashSet<string> likedUsers = new HashSet<string>();
        private HashSet<string> dislikedUsers = new HashSet<string>();
        private HashSet<string> watchLaterUsers = new HashSet<string>();
        private HashSet<string> watchedUsers = new HashSet<string>();

        public int Likes
        {
            get { return likedUsers.Count; }
        }

        public int Dislikes
        {
            get { return dislikedUsers.Count; }
        }

        public int WatchLater
        {
            get { return watchLaterUsers.Count; }
        }

        public int Watched
        {
            get { return watchedUsers.Count; }
        }

        public int Score
        {
            get
            {
                return (Likes * 3) + WatchLater - Dislikes - (Watched * 2);
            }
        }

        public Result(Movies movie)
        {
            Movie = movie;
        }

        public void AddSwipe(string username, SwipeType swipeType)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                username = "Unbekannt";
            }

            switch (swipeType)
            {
                case SwipeType.Like:
                    likedUsers.Add(username);
                    break;

                case SwipeType.Dislike:
                    dislikedUsers.Add(username);
                    break;

                case SwipeType.WatchLater:
                    watchLaterUsers.Add(username);
                    break;

                case SwipeType.Watched:
                    watchedUsers.Add(username);
                    break;
            }
        }
    }
}