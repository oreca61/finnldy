using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class Swipe
    {
        public User User { get; private set; }
        public Movies Movies { get; private set; }

        public SwipeType Type { get; private set; }

        public Swipe(User user, Movies movies, SwipeType type)
        {
            User = user;
            Movies = movies;
            Type = type;
        }

        public bool IsLiked()
        {
            return Type == SwipeType.Like;
        }

        public bool IsDisliked()
        {
            return Type == SwipeType.Dislike;
        }

        public bool IsWatched()
        {
            return Type == SwipeType.Watched;
        }

        public bool IsWatchedLater()
        {
            return Type == SwipeType.WatchLater;
        }
    }
}
