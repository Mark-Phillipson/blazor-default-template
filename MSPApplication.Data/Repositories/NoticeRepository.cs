using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
    public class NoticeRepository : INoticeRepository
    {
        private readonly AppDbContext _appDbContext;
        public NoticeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public Notice AddNotice(Notice notice)
        {
            var addedEntity = _appDbContext.Notices.Add(notice);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public void DeleteNotice(int noticeId)
        {
            var foundNotice = _appDbContext.Notices.FirstOrDefault(e => e.NoticeId == noticeId);
            if (foundNotice == null)
            {
                return;
            }
            _appDbContext.Notices.Remove(foundNotice);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<Notice> GetAllNotices()
        {
            var result = _appDbContext.Notices;
            return result;
        }

        public Notice GetNoticeById(int noticeId)
        {
            var result = _appDbContext.Notices.FirstOrDefault(c => c.NoticeId == noticeId);
            return result;
        }

        public Notice UpdateNotice(Notice notice)
        {
            var foundNotice = _appDbContext.Notices.FirstOrDefault(e => e.NoticeId == notice.NoticeId);
            if (foundNotice != null)
            {
                foundNotice.DatePosted = notice.DatePosted;
                foundNotice.Description = notice.Description;
                foundNotice.Priority = notice.Priority;
                foundNotice.Show = notice.Show;
                _appDbContext.SaveChanges();
                return foundNotice;
            }
            return null;
        }
    }
}
