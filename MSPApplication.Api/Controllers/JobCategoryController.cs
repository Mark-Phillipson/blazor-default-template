using MSPApplication.Api.Models;
using Microsoft.AspNetCore.Mvc;
using MSPApplication.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobCategoryController : Controller
    {
        private readonly IJobCategoryRepository _jobCategoryRepository;

        public JobCategoryController(IJobCategoryRepository jobCategoryRepository)
        {
            _jobCategoryRepository = jobCategoryRepository;
        }


        // GET: api/<controller>
        [HttpGet]
        public IActionResult GetJobCategories()
        {
            return Ok(_jobCategoryRepository.GetAllJobCategories());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult GetJobCategoryById(int id)
        {
            var result = _jobCategoryRepository.GetJobCategoryById(id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateJobCategory([FromBody] JobCategory jobCategory)
        {
            if (jobCategory == null)
                return BadRequest();

            if (jobCategory.JobCategoryName == string.Empty )
            {
                ModelState.AddModelError("JobCategoryName", "The Job Category Name Is Required!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdJobCategory = _jobCategoryRepository.AddJobCategory(jobCategory);

            return Created("JobCategory", createdJobCategory);
        }

        [HttpPut]
        public IActionResult UpdateJobCategory([FromBody] JobCategory jobCategory)
        {
            if (jobCategory == null)
                return BadRequest();

            if (jobCategory.JobCategoryName == string.Empty)
            {
                ModelState.AddModelError("JobCategoryName", "The Job Category Name Is Required!");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var jobCategoryToUpdate = _jobCategoryRepository.GetJobCategoryById(jobCategory.JobCategoryId);

            if (jobCategoryToUpdate == null)
                return NotFound();

            _jobCategoryRepository.UpdateJobCategory(jobCategory);

            return NoContent(); //success
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteJobCategory(int id)
        {
            if (id == 0)
                return BadRequest();

            var jobCategoryToDelete = _jobCategoryRepository.GetJobCategoryById(id);
            if (jobCategoryToDelete == null)
                return NotFound();

            _jobCategoryRepository.DeleteJobCategory(id);

            return NoContent();//success
        }

    }
}
