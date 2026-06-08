using System;
using System.Collections.Generic;
using System.Text;

namespace Finnldy.BLL
{
    public class Movies
    {
        public int ApiMovieId { get; set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public List<int> GenreIds { get; private set; }

        public string Cover { get; private set; }
        public string ReleaseDate { get; set; }

        public Movies(int apiMovieId, string name, string description, List<int> genreIds, string cover, string releaseDate)
        {
            ApiMovieId = apiMovieId;
            Name = name;
            Description = description;
            GenreIds = genreIds;
            Cover = cover;
            ReleaseDate = releaseDate;
        }

        public Movies(string name)
        {
            Name = name;
            GenreIds = new List<int>();
        }
    }
}