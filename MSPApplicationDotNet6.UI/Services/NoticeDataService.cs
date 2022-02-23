using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplicationDotNet6.UI.Services
{
    public class NoticeDataService : INoticeDataService
    {
        private readonly HttpClient _httpClient;

        public NoticeDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Notice>> GetAllNotices()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Notice>>
                (await _httpClient.GetStreamAsync($"api/notice"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<Notice> GetNoticeById(int noticeId)
        {
            return await JsonSerializer.DeserializeAsync<Notice>
                (await _httpClient.GetStreamAsync($"api/notice/{noticeId}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
        public async Task<Notice> AddNotice(Notice notice)
        {
            var noticeJson =
                new StringContent(JsonSerializer.Serialize(notice), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/notice", noticeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Notice>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task UpdateNotice(Notice notice)
        {
            var noticeJson =
                new StringContent(JsonSerializer.Serialize(notice), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync($"api/notice/{notice.NoticeId}", noticeJson);
        }

        public async Task DeleteNotice(int noticeId)
        {
            await _httpClient.DeleteAsync($"api/notice/{noticeId}");
        }

    }
}
