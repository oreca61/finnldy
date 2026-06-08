using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Finnldy.DAL
{
    public class GetMovies
    {
        private string apiKey = "4e4f43d91ee6bbb1f39fa6e4a1b4e517";

        public async Task<List<Movies>> GetPopularMoviesAsync()
        {
            List<Movies> movies = new List<Movies>();

            using HttpClient client = new HttpClient();

            for (int page = 1; page <= 100; page++)
            {
                string url = $"https://api.themoviedb.org/3/movie/popular?api_key={apiKey}&language=de-DE&page={page}";

                string result = await client.GetStringAsync(url);

                JsonDocument jsonDocument = JsonDocument.Parse(result);

                JsonElement root = jsonDocument.RootElement;
                JsonElement results = root.GetProperty("results");

                foreach (JsonElement movieJson in results.EnumerateArray())
                {
                    int id = movieJson.GetProperty("id").GetInt32();

                    string title = movieJson.GetProperty("title").GetString();

                    string description = movieJson.GetProperty("overview").GetString();

                    string releaseDate = movieJson.GetProperty("release_date").GetString();

                    List<int> genreIds = new List<int>();

                    if (movieJson.TryGetProperty("genre_ids", out JsonElement genreArray))
                    {
                        foreach (JsonElement genreIdJson in genreArray.EnumerateArray())
                        {
                            genreIds.Add(genreIdJson.GetInt32());
                        }
                    }

                    string posterPath = "";

                    if (movieJson.GetProperty("poster_path").ValueKind != JsonValueKind.Null)
                    {
                        posterPath = movieJson.GetProperty("poster_path").GetString();
                    }

                    string cover = "";

                    if (!string.IsNullOrEmpty(posterPath))
                    {
                        cover = "https://image.tmdb.org/t/p/w500" + posterPath;
                    }

                    Movies movie = new Movies(
                        id,
                        title,
                        description,
                        genreIds,
                        cover,
                        releaseDate
                    );

                    movies.Add(movie);
                }
            }

            return movies;
        }
    }
}

