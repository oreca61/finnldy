using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Finnldy.DAL
{
    public class GetMovies
    {
        private string apiKey = "4e4f43d91ee6bbb1f39fa6e4a1b4e517";

        public async Task<string> GetPopularMoviesJsonAsync()
        {
            using HttpClient client = new HttpClient();

            string url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=de-DE&page=1";

            string result = await client.GetStringAsync(url);

            return result;
        }
    }
}

