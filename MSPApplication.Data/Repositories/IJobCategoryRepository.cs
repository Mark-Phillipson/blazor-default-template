using System.Collections.Generic;
using MSPApplication.Shared;

namespace MSPApplication.Data.Repositories
{
    public interface IJobCategoryRepository
    {
        IEnumerable<JobCategory> GetAllJobCategories();
        JobCategory GetJobCategoryById(int jobCategoryId);
        JobCategory AddJobCategory(JobCategory jobCategory);
        JobCategory UpdateJobCategory(JobCategory jobCategory);
        void DeleteJobCategory(int jobCategoryId);
    }
}
