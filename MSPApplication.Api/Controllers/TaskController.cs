using Microsoft.AspNetCore.Mvc;
using MSPApplication.Api.Models;
using MSPApplication.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_taskRepository.GetAllTasks());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetTaskById(int id)
        {
            var result = _taskRepository.GetTaskById(id);
            return Ok(result);
        }

        [HttpPut]
        public IActionResult UpdateTask([FromBody] HRTask task)
        {
            if (task.HRTaskId > 0)
            {
                var taskToUpdate = _taskRepository.GetTaskById(task.HRTaskId);

                if (taskToUpdate == null)
                    return NotFound();

                var updatedTask = _taskRepository.UpdateTask(task);
                if (updatedTask == null)
                {
                    return NotFound();
                }
                return NoContent(); //success
            }
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Post(HRTask task)
        {
            if (task == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createdTask = _taskRepository.AddTask(task);
            return Created("Task", createdTask);
        }
    }
}
