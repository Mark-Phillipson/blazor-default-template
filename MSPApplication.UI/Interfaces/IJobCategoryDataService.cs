using System.Collections.Generic;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplication.UI.Services
{
    public interface IJobCategoryDataService
    {
        Task<IEnumerable<JobCategory>> GetAllJobCategories();
        Task<JobCategory> GetJobCategoryById(int jobCategoryId);
        Task<JobCategory> AddJobCategory(JobCategory jobCategory);
        Task UpdateJobCategory(JobCategory jobCategory);
        Task DeleteJobCategory(int jobCategoryId);
    }
}
