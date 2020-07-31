using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplication.UI.Services
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
