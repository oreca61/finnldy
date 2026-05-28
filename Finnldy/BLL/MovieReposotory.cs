using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Finnldy.BLL
{
    public class MovieReposotory
    {
        public List<Movies> movies = new List<Movies>();

        public List<Movies> GetAllMovies() 
        {
            return movies;
        }

        public void AddMovie(Movies movie)
        {
            movies.Add(movie);
        }

        public Movies FindMovieById(int id)
        {
            return movies.FirstOrDefault(movie => movie.ApiMovieId == id);
        }

    }
}
