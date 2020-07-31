using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly INoticeRepository _noticeRepository;
        public NoticeController(INoticeRepository noticeRepository)
        {
            _noticeRepository = noticeRepository;
        }
        // GET: api/<NoticesController>
        [HttpGet]
        public IActionResult GetAllNotices()
        {
            var result = _noticeRepository.GetAllNotices();
            return Ok(result);
        }

        // GET api/<NoticesController>/5
        [HttpGet("{id}")]
        public IActionResult GetNoticeById(int id)
        {
            var result = _noticeRepository.GetNoticeById(id);
            return Ok(result);
        }

        // POST api/<NoticesController>
        [HttpPost]
        public IActionResult CreateNotice([FromBody] Notice notice)
        {
            if (notice == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(notice.Description))
            {
                ModelState.AddModelError("Description", "The description should not be empty! ");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdNote = _noticeRepository.AddNotice(notice);
            return Created("notice", createdNote);
        }

        // PUT api/<NoticesController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateNotice([FromBody] Notice notice)
        {
            if (notice == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(notice.Description))
            {
                ModelState.AddModelError("Description", "The description should not be empty! ");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var noticeToUpdate = _noticeRepository.GetNoticeById(notice.NoticeId);
            if (noticeToUpdate == null)
            {
                return NotFound();
            }
            _noticeRepository.UpdateNotice(notice);
            return NoContent();//success
        }

        // DELETE api/<NoticesController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteNotice(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var noticeToDelete = _noticeRepository.GetNoticeById(id);
            if (noticeToDelete == null) return NotFound();
            _noticeRepository.DeleteNotice(id);
            return NoContent();//success
        }
    }
}
