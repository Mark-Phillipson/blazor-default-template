using MSPApplication.Shared;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
    public class TaskDataService : ITaskDataService
    {
        private readonly HttpClient _httpClient;

        public TaskDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HRTask> AddTask(HRTask task)
        {
            var taskJson =
                new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/task", taskJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<HRTask>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteTask(int taskId)
        {
            await _httpClient.DeleteAsync($"api/task/{taskId}");
        }

        public async Task<IEnumerable<HRTask>> GetAllTasks()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<HRTask>>
                (await _httpClient.GetStreamAsync($"api/task"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<HRTask> GetTaskById(int taskId)
        {
            //        var result = await JsonSerializer.DeserializeAsync<Employee>
            //(await _httpClient.GetStreamAsync($"api/employee/{employeeId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            var result = await JsonSerializer.DeserializeAsync<HRTask>
                (await _httpClient.GetStreamAsync($"api/task/{taskId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return result;
        }
        public async Task UpdateTask(HRTask task)
        {
            var taskJson =
    new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/task", taskJson);

        }
    }
}
