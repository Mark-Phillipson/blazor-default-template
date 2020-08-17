using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;

// TODO Create unit tests for this controller API
namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDetailController : ControllerBase
    {
        private readonly ICompanyDetailRepository _companyDetailRepository;
        public CompanyDetailController(ICompanyDetailRepository companyDetailRepository)
        {
            _companyDetailRepository = companyDetailRepository;
        }
        // GET: api/<CompanyDetailsController>
        [HttpGet]
        public IActionResult GetAllCompanyDetails()
        {
            var result = _companyDetailRepository.GetAllCompanyDetails();
            return Ok(result);
        }

        // GET api/<CompanyDetailsController>/5
        [HttpGet("{id}")]
        public IActionResult GetCompanyDetailById(int id)
        {
            var result = _companyDetailRepository.GetCompanyDetailById(id);
            return Ok(result);
        }

        // POST api/<CompanyDetailsController>
        [HttpPost]
        public IActionResult CreateCompanyDetail([FromBody] CompanyDetail companyDetail)
        {
            if (companyDetail == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(companyDetail.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "The Company Name should not be empty!");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdCompanyDetail = _companyDetailRepository.AddCompanyDetail(companyDetail);
            return Created("companyDetail", createdCompanyDetail);
        }

        // PUT api/<CompanyDetailsController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateCompanyDetail([FromBody] CompanyDetail companyDetail)
        {
            if (companyDetail == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(companyDetail.CompanyName))
            {
                ModelState.AddModelError("CompanyName", "The Company Name should not be empty!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var companyDetailToUpdate = _companyDetailRepository.GetCompanyDetailById(companyDetail.Id);
            if (companyDetailToUpdate == null)
            {
                return NotFound();
            }
            _companyDetailRepository.UpdateCompanyDetail(companyDetail);
            return NoContent();//success
        }

        // DELETE api/<CompanyDetailsController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCompanyDetail(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var companyDetailToDelete = _companyDetailRepository.GetCompanyDetailById(id);
            if (companyDetailToDelete == null) return NotFound();
            _companyDetailRepository.DeleteCompanyDetail(id);
            return NoContent();//success
        }
    }
}
