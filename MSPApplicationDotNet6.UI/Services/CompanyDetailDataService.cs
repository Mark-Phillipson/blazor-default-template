using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplicationDotNet6.UI.Services
{
    public class CompanyDetailDataService : ICompanyDetailDataService
    {
        private readonly HttpClient _httpClient;

        public CompanyDetailDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<CompanyDetail>> GetAllCompanyDetails()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<CompanyDetail>>
                (await _httpClient.GetStreamAsync($"api/companydetail"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<CompanyDetail> GetCompanyDetailById(int id)
        {
            return await JsonSerializer.DeserializeAsync<CompanyDetail>
                (await _httpClient.GetStreamAsync($"api/companydetail/{id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<CompanyDetail> AddCompanyDetail(CompanyDetail companyDetail)
        {
            var companyDetailJson =
                new StringContent(JsonSerializer.Serialize(companyDetail), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/companydetail", companyDetailJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<CompanyDetail>(await response.Content.ReadAsStreamAsync());
            }
            return null;
        }

        public async Task UpdateCompanyDetail(CompanyDetail companyDetail)
        {
            var companyDetailJson =
                new StringContent(JsonSerializer.Serialize(companyDetail), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/companydetail/{companyDetail.Id}", companyDetailJson);
        }

        public async Task DeleteCompanyDetail(int id)
        {
            await _httpClient.DeleteAsync($"api/companydetail/{id}");
        }

    }
}
