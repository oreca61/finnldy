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
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public double Popularity { get; set; }
        public string OriginalLanguage { get; set; }
        public bool Adult { get; set; }

        public Movies(int apiMovieId, string name, string description, List<int> genreIds, string cover, string releaseDate, string originalLanguage, bool adult, double voteAverage)
        {
            ApiMovieId = apiMovieId;
            Name = name;
            Description = description;
            GenreIds = genreIds;
            Cover = cover;
            ReleaseDate = releaseDate;
            OriginalLanguage = originalLanguage;
            Adult = adult;
            VoteAverage = voteAverage;
        }

        public Movies(string name)
        {
            Name = name;
            Description = "";
            GenreIds = new List<int>();
            Cover = "";
            ReleaseDate = "";
            OriginalLanguage = "";
            Adult = false;
        }
    }
}