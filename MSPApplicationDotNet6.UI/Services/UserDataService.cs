using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using SendGrid;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
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
            Stream jasonResult = await _httpClient.GetStreamAsync($"api/user");
            //string value;
            //using (var reader = new StreamReader(jasonResult, true))
            //{
            //    value = reader.ReadToEnd();
            //}

            return await JsonSerializer.DeserializeAsync<IEnumerable<AspNetUser>>
                (jasonResult, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<AspNetUser>> GetAllUsersInRole(string id)
        {
            var url = $"api/user/getallusersinrole/{id}";
            return await JsonSerializer.DeserializeAsync<IEnumerable<AspNetUser>>
                (await _httpClient.GetStreamAsync(url), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<AspNetRole>> GetAllRolesForUser(string userId)
        {
            var url = $"api/user/getallrolesforuser/{userId}";
            return await JsonSerializer.DeserializeAsync<IEnumerable<AspNetRole>>
                (await _httpClient.GetStreamAsync(url), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        }

        public async Task<AspNetUser> GetUserById(string id)
        {
            var url = $"api/user/getuserbyid/{id}";
            return await JsonSerializer.DeserializeAsync<AspNetUser>
                (await _httpClient.GetStreamAsync(url), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
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
        public async Task DeleteUserRole(string userId, string roleId)
        {
            var result = await _httpClient.DeleteAsync($"api/user?userId={userId}&roleId={roleId}");
            if (!result.IsSuccessStatusCode)
            {
                throw new System.Exception($"Failed to delete:{result}");
            }
            System.Console.WriteLine(result);

        }
        public async Task<AspNetUserRole> AddUserRole(string userId, string roleId)
        {
            var roleUser = new AspNetUserRole() { UserId = userId, RoleId = roleId };
            var roleUserJson =
                new StringContent(JsonSerializer.Serialize(roleUser), Encoding.UTF8, "application/json");

            var result = await _httpClient.PostAsync($"api/user/adduserrole", roleUserJson);
            if (result.IsSuccessStatusCode)
{
                return await JsonSerializer.DeserializeAsync<AspNetUserRole>(await result.Content.ReadAsStreamAsync());
            }
            return null;

        }

    }
}
