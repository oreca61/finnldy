using Finnldy.BLL;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net;

namespace Finnldy.DAL.Database
{
    public class Database
    {
        private readonly HttpClient _httpClient;

        public Database()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8000");
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _httpClient.GetFromJsonAsync<List<User>>("/users/");
            return users ?? new List<User>();
        }

        public async Task<User?> GetUser(string username)
        {
            string encodedUsername = Uri.EscapeDataString(username);

            var response = await _httpClient.GetAsync($"/users/by-name/{encodedUsername}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<User?> CreateUser(string name)
        {
            var request = new 
            {
                name = name
            };

            var response = await _httpClient.PostAsJsonAsync("/users/", request);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<User>();
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var response = await _httpClient.DeleteAsync($"/users/{userId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Swipe>> GetUserSwipes(int userId)
        {
            var swipes = await _httpClient.GetFromJsonAsync<List<Swipe>>($"/users/{userId}/swipes");
            return swipes ?? new List<Swipe>();
        }

        public async Task<List<Swipe>> GetUserWatchedMovies(int userId)
        {
            var watched = await _httpClient.GetFromJsonAsync<List<Swipe>>($"/users/{userId}/watched");
            return watched ?? new List<Swipe>();
        }


        public async Task<bool> IsUserActive(string name)
            {
                var response = await _httpClient.GetAsync($"/users/by-name/{name}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return true;
                }

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }

                throw new Exception("Fehler beim Prüfen, ob der User existiert.");
            }
    }
}
