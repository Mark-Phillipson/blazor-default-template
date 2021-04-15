using Microsoft.EntityFrameworkCore;
using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly AppDbContext _appDbContext;
		private readonly ITaskRepository _taskRepository;

		public EmployeeRepository(AppDbContext appDbContext, ITaskRepository taskRepository)
		{
			_appDbContext = appDbContext;
			_taskRepository = taskRepository;
		}

		public IEnumerable<Employee> GetAllEmployees(int? jobCategoryId = null)
		{
			if (jobCategoryId != null)
			{
				return _appDbContext.Employees.Include(i => i.JobCategory).Where(v => v.JobCategoryId == jobCategoryId);
			}
			return _appDbContext.Employees.Include(i => i.JobCategory);
		}

		public Employee GetEmployeeById(int employeeId)
		{
			return _appDbContext.Employees.Include(i => i.HRTasks).FirstOrDefault(c => c.EmployeeId == employeeId);
		}

		public Employee AddEmployee(Employee employee)
		{
			var addedEntity = _appDbContext.Employees.Add(employee);
			_appDbContext.SaveChanges();
			return addedEntity.Entity;
		}

		public Employee UpdateEmployee(Employee employee)
		{
			var foundEmployee = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

			if (foundEmployee != null)
			{
				foundEmployee.CountryId = employee.CountryId;
				foundEmployee.MaritalStatus = employee.MaritalStatus;
				foundEmployee.BirthDate = employee.BirthDate;
				foundEmployee.City = employee.City;
				foundEmployee.Email = employee.Email;
				foundEmployee.FirstName = employee.FirstName;
				foundEmployee.LastName = employee.LastName;
				foundEmployee.Gender = employee.Gender;
				foundEmployee.PhoneNumber = employee.PhoneNumber;
				foundEmployee.Smoker = employee.Smoker;
				foundEmployee.Street = employee.Street;
				foundEmployee.Zip = employee.Zip;
				foundEmployee.JobCategoryId = employee.JobCategoryId;
				foundEmployee.Comment = employee.Comment;
				foundEmployee.ExitDate = employee.ExitDate;
				foundEmployee.JoinedDate = employee.JoinedDate;
				foundEmployee.Latitude = employee.Latitude;
				foundEmployee.Longitude = employee.Longitude;
				foreach (var task in employee.HRTasks)
				{
					if (task.HRTaskId > 0)
					{
						_taskRepository.UpdateTask(task);
					}
					else if (task.HRTaskId == 0)
					{
						_taskRepository.AddTask(task);
					}
				}
				_appDbContext.SaveChanges();



				return foundEmployee;
			}

			return null;
		}

		public void DeleteEmployee(int employeeId)
		{
			var foundEmployee = _appDbContext.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
			if (foundEmployee == null) return;

			_appDbContext.Employees.Remove(foundEmployee);
			_appDbContext.SaveChanges();
		}
	}
}
