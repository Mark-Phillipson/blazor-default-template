using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Services
{
    public interface ITaskDataService
    {
        Task<IEnumerable<HRTask>> GetAllTasks();
        Task<HRTask> GetTaskById(int taskId);
        Task<HRTask> AddTask(HRTask task);
        Task UpdateTask(HRTask task);
        Task DeleteTask(int taskId);
    }
}
