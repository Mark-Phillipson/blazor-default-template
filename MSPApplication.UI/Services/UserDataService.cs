using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplication.UI.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly HttpClient _httpClient;

        public UserDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AspNetUser>> GetAllUsers()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<AspNetUser>>
                (await _httpClient.GetStreamAsync($"api/user"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<AspNetUser> GetUserById(string id)
        {
            return await JsonSerializer.DeserializeAsync<AspNetUser>
                (await _httpClient.GetStreamAsync($"api/user/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<AspNetUser> AddUser(AspNetUser user)
        {
            var userJson =
                new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/user", userJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<AspNetUser>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateUser(AspNetUser user)
        {
            var userJson =
                new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/user/{user.Id}", userJson);
        }

        public async Task DeleteUser(string id)
        {
            await _httpClient.DeleteAsync($"api/user/{id}");
        }

    }
}
