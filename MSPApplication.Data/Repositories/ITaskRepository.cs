using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplication.Data.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<HRTask> GetAllTasks();
        HRTask GetTaskById(int taskId);
        HRTask AddTask(HRTask task);
        HRTask UpdateTask(HRTask task);
        void DeleteTask(int id);
    }
}
