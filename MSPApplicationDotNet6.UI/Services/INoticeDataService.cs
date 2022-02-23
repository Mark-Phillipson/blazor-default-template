using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
    public interface INoticeDataService
    {
        Task<Notice> AddNotice(Notice notice);
        Task DeleteNotice(int noticeId);
        Task<IEnumerable<Notice>> GetAllNotices();
        Task<Notice> GetNoticeById(int noticeId);
        Task UpdateNotice(Notice notice);
    }
}