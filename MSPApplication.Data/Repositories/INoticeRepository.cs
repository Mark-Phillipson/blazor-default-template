using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface INoticeRepository
    {
        IEnumerable<Notice> GetAllNotices();
        Notice GetNoticeById(int noticeId);
        Notice AddNotice(Notice notice);
        Notice UpdateNotice(Notice notice);
        void DeleteNotice(int noticeId);
    }
}
