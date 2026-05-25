using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Finnldy.BLL
{
    public class User
    {
        public string Name { get; private set; }
        private List<Movies> liked = new List<Movies>();
        private List<Movies> disliked = new List<Movies>();
        private List<Movies> watched = new List<Movies>();
        private List<Movies> watchlater = new List<Movies>();
        private string genre;



        public User(string name)
        {
            this.Name = name;
        }

        public void LikeMovie(Movies movie)
        {
            liked.Add(movie);
        }
        public void DislikeMovie(Movies movie)
        {
            disliked.Add(movie);
        }
        public void AddWatchedMovie(Movies movie)
        {
            watched.Add(movie);
        }
        public void AddWatchLaterMovie(Movies movie)
        {
            watchlater.Add(movie);
        }
    }
}