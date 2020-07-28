using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Api.Models
{
    public class JobCategoryRepository : IJobCategoryRepository
    {
        private readonly AppDbContext _appDbContext;

        public JobCategoryRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<JobCategory> GetAllJobCategories()
        {
            return _appDbContext.JobCategories.Include(i => i.Employees);
        }

        public JobCategory GetJobCategoryById(int jobCategoryId)
        {
            return _appDbContext.JobCategories.Include(i => i.Employees).FirstOrDefault(c => c.JobCategoryId == jobCategoryId);
        }
        public JobCategory AddJobCategory(JobCategory jobCategory)
        {
            var addedEntity = _appDbContext.JobCategories.Add(jobCategory);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }
        public JobCategory UpdateJobCategory(JobCategory jobCategory)
        {
            var foundJobCategory = _appDbContext.JobCategories.FirstOrDefault(e => e.JobCategoryId == jobCategory.JobCategoryId);

            if (foundJobCategory != null)
            {
                foundJobCategory.JobCategoryName = jobCategory.JobCategoryName;
                _appDbContext.SaveChanges();
                return foundJobCategory;
            }
            return null;
        }

        public void DeleteJobCategory(int jobCategoryId)
        {
            var foundJobCategory = _appDbContext.JobCategories.FirstOrDefault(e => e.JobCategoryId == jobCategoryId);
            if (foundJobCategory == null)
            {
                return;
            }
            _appDbContext.JobCategories.Remove(foundJobCategory);
            _appDbContext.SaveChanges();
        }
    }
}