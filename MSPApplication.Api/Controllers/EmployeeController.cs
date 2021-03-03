using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;

namespace MSPApplication.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : Controller
	{
		private readonly IEmployeeRepository _employeeRepository;

		public EmployeeController(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		/// <summary> 
		/// Lists all the employees in the system and the job category is actually optional!
		/// </summary> 
		/// <param name="jobCategoryId"></param>
		[Route("[action]/{jobCategoryId?}")]
		[Route("[action]")]
		[HttpGet]
		public ActionResult<IEnumerable<Employee>> GetAllEmployees(int? jobCategoryId = null)
		{
			var result = _employeeRepository.GetAllEmployees(jobCategoryId);
			if (result!= null )
			{
				return Ok(result);
			}

			return new BadRequestObjectResult( new Exception("there are no employees in the system!"));
			// How to inspect action results in tests
			//StatusCodeResult code;
			//if ((code as StatusCodeResult).StatusCode == 200)
			//{
				
			//}
		}

		[Route("[action]/{id}")]
		[HttpGet]
		public IActionResult GetEmployeeById(int id)
		{
			var result = _employeeRepository.GetEmployeeById(id);
			return Ok(result);
		}

		[HttpPost]
		public IActionResult CreateEmployee([FromBody] Employee employee)
		{
			if (employee == null)
				return BadRequest();

			if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
			{
				ModelState.AddModelError("Name/FirstName", "The name or first name shouldn't be empty");
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var createdEmployee = _employeeRepository.AddEmployee(employee);

			return Created("employee", createdEmployee);
		}

		[HttpPut]
		public IActionResult UpdateEmployee([FromBody] Employee employee)
		{
			if (employee == null)
				return BadRequest();

			if (employee.FirstName == string.Empty || employee.LastName == string.Empty)
			{
				ModelState.AddModelError("LastName/FirstName", "The first and last name are required!");
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var employeeToUpdate = _employeeRepository.GetEmployeeById(employee.EmployeeId);

			if (employeeToUpdate == null)
				return NotFound();

			_employeeRepository.UpdateEmployee(employee);

			return NoContent(); //success
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteEmployee(int id)
		{
			if (id == 0)
				return BadRequest();

			var employeeToDelete = _employeeRepository.GetEmployeeById(id);
			if (employeeToDelete == null)
				return NotFound();

			_employeeRepository.DeleteEmployee(id);

			return NoContent();//success
		}
	}
}
