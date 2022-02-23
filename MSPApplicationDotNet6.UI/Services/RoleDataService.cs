using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplicationDotNet6.UI.Services
{
    public class RoleDataService : IRoleDataService
    {
        private readonly HttpClient _httpClient;

        public RoleDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<AspNetRole>> GetAllRoles()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<AspNetRole>>
                (await _httpClient.GetStreamAsync($"api/role"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<AspNetRole> GetRoleById(string id)
        {
            return await JsonSerializer.DeserializeAsync<AspNetRole>
                (await _httpClient.GetStreamAsync($"api/role/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<AspNetRole> AddRole(AspNetRole role)
        {
            var roleJson =
                new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/role", roleJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<AspNetRole>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateRole(AspNetRole role)
        {
            var roleJson =
                new StringContent(JsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/role/{role.Id}", roleJson);
        }

        public async Task DeleteRole(string id)
        {
            await _httpClient.DeleteAsync($"api/role/{id}");
        }

    }
}
