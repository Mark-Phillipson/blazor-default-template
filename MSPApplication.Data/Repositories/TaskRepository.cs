using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _appDbContext;

        public TaskRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<HRTask> GetAllTasks()
        {
            return _appDbContext.Tasks.Include(i => i.Employee);
        }

        public HRTask GetTaskById(int TaskId)
        {
            HRTask task = _appDbContext.Tasks.FirstOrDefault(c => c.HRTaskId == TaskId);
            return task;
        }

        public HRTask AddTask(HRTask task)
        {
            var addedEntity = _appDbContext.Tasks.Add(task);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }
        public HRTask UpdateTask(HRTask task)
        {
            var foundTask = _appDbContext.Tasks.FirstOrDefault(e => e.HRTaskId == task.HRTaskId);
            if (foundTask != null)
            {
                foundTask.EmployeeId = task.EmployeeId;
                foundTask.Description = task.Description;
                foundTask.Status = task.Status;
                foundTask.Title = task.Title;
                _appDbContext.SaveChanges();
                return foundTask;
            }
            return null;
        }

        public void DeleteTask(int id)
        {
            var taskToDelete = _appDbContext.Tasks.Where(v => v.HRTaskId == id).FirstOrDefault();
            _appDbContext.Tasks.Remove(taskToDelete);
            _appDbContext.SaveChanges();
        }
    }
}
