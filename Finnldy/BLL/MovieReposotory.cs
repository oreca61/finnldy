using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class MovieReposotory
    {
        private List<Movies> movies = new List<Movies>();

        public List<Movies> GetAllMovies() 
        {
            return movies;
        }

        public void AddMovie(Movies movie)
        {
            movies.Add(movie);
        }

    }
}
