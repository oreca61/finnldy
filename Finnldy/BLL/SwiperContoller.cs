using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class SwiperContoller
    {
        public Swipe LikeMovie(User user,Movies movie)
        {
            user.LikeMovie(movie);

            Swipe swipe = new Swipe(user, movie, SwipeType.Like);
            return swipe;
        }

        public Swipe DislikeMovie(User user, Movies movie)
        {
            user.DislikeMovie(movie);
            
            Swipe swipe = new Swipe(user, movie, SwipeType.Dislike);
            return swipe;
        }

        public Swipe AddWatchedMovie(User user, Movies movie)
        {
            user.AddWatchedMovie(movie);

            Swipe swipe = new Swipe(user, movie, SwipeType.Watched);
            return swipe;
        }

        public Swipe AddWatchLaterMovie(User user, Movies movie)
        {
            user.AddWatchLaterMovie(movie);
            
            Swipe swipe = new Swipe(user, movie, SwipeType.WatchLater);
            return swipe;
        }
    }
}
