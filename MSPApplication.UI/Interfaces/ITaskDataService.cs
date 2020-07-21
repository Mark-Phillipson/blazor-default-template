using System.Collections.Generic;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplication.UI.Services
{
    public interface ITaskDataService
    {
        Task<IEnumerable<HRTask>> GetAllTasks();
        Task<HRTask> GetTaskById(int taskId);
        Task<HRTask> AddTask(HRTask task);
    }
}
