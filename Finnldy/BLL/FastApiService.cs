using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Finnldy.DAL
{

    // Mit Ki hilfe um besser die Verbindung zwischen DBI und POS zu schaffen
    public class FastApiService : ISwipeApiService
    {
        private readonly HttpClient httpClient = new HttpClient();

        private const string BaseUrl = "http://127.0.0.1:8000";
        private const string AdminApiKey = "admin";
        private const string UserApiKey = "user";


        // Ki Hilfe Anfang 
        // Schreibe eine Methode, die einen Swipe aus der C#-App per HTTP POST an meine FastAPI sendet.
        public async Task<bool> SaveSwipeAsync(User user, Movies movie, SwipeType swipeType)
        {
            if (user == null || movie == null)
            {
                return false;
            }

            var data = new
            {
                username = user.Name,
                api_movie_id = movie.ApiMovieId,
                movie_title = movie.Name,
                swipe_type = swipeType.ToString()
            };

            string json = JsonSerializer.Serialize(data);

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Post,
                BaseUrl + "/swipes/"
            );

            request.Headers.Add("X-API-Key", AdminApiKey);

            request.Content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    AppLogger.Info("Swipe erfolgreich an FastAPI gesendet: "
                                   + movie.Name + " / " + swipeType);
                }
                else
                {
                    AppLogger.Error("FastAPI Fehler beim Speichern von Swipe. Status: "
                                    + response.StatusCode);
                }

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                AppLogger.Error("FastAPI nicht erreichbar: " + ex.Message);
                return false;
            }
        }

        // Ki Hilfe Ende

        public async Task<List<ApiMovieResult>> GetResultsAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Get,
                BaseUrl + "/swipes/results"
            );

            request.Headers.Add("X-API-Key", UserApiKey);

            try
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return new List<ApiMovieResult>();
                }

                string json = await response.Content.ReadAsStringAsync();

                List<ApiMovieResult>? results = JsonSerializer.Deserialize<List<ApiMovieResult>>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }
                );

                return results ?? new List<ApiMovieResult>();
            }
            catch
            {
                return new List<ApiMovieResult>();
            }
        }
    }

    public class ApiMovieResult
    {
        [JsonPropertyName("movie_id")]
        public int MovieId { get; set; }

        [JsonPropertyName("api_movie_id")]
        public int ApiMovieId { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("likes")]
        public int Likes { get; set; }

        [JsonPropertyName("dislikes")]
        public int Dislikes { get; set; }

        [JsonPropertyName("watched")]
        public int Watched { get; set; }

        [JsonPropertyName("watch_later")]
        public int WatchLater { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}