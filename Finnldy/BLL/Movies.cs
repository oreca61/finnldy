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
        public string Genre { get; private set; }
        public string Cover { get; private set; }

        public Movies(string name, string genre)
        {
            this.Name = name;
            this.Genre = genre;
        }
    }
}
