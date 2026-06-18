using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Finnldy.BLL
{
    public class User
    {
        public string Name { get; private set; }
        public List<Movies> LikedMovies { get; private set; }
        public List<Movies> DislikedMovies { get; private set; }
        public List<Movies> WatchedMovies { get; private set; }
        public List<Movies> WatchLaterMovies { get; private set; }

        public User(string name)
        {
            Name = name;


            LikedMovies = new List<Movies>();
            DislikedMovies = new List<Movies>();
            WatchedMovies = new List<Movies>();
            WatchLaterMovies = new List<Movies>();
        }

        public void LikeMovie(Movies movie)
        {
            LikedMovies.Add(movie);
        }

        public void DislikeMovie(Movies movie)
        {
            DislikedMovies.Add(movie);
        }

        public void AddWatchedMovie(Movies movie)
        {
            WatchedMovies.Add(movie);
        }

        public void AddWatchLaterMovie(Movies movie)
        {
            WatchLaterMovies.Add(movie);
        }
    }
}