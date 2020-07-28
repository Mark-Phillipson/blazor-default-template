using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplication.UI.Services
{
    public class JobCategoryDataService : IJobCategoryDataService
    {
        private readonly HttpClient _httpClient;

        public JobCategoryDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<JobCategory>> GetAllJobCategories()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<JobCategory>>
                (await _httpClient.GetStreamAsync($"api/jobcategory"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<JobCategory> GetJobCategoryById(int jobCategoryId)
        {
            return await JsonSerializer.DeserializeAsync<JobCategory>
                (await _httpClient.GetStreamAsync($"api/jobcategory/{jobCategoryId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<JobCategory> AddJobCategory(JobCategory jobCategory)
        {
            var jobCategoryJson =
                new StringContent(JsonSerializer.Serialize(jobCategory), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/jobcategory", jobCategoryJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<JobCategory>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateJobCategory(JobCategory jobCategory)
        {
            var jobCategoryJson =
                new StringContent(JsonSerializer.Serialize(jobCategory), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/jobcategory", jobCategoryJson);
        }

        public async Task DeleteJobCategory(int jobCategoryId)
        {
            await _httpClient.DeleteAsync($"api/jobcategory/{jobCategoryId}");
        }

    }
}
