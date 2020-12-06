using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
	public partial class EmployeeEdit
	{
		[Inject]
		public IEmployeeDataService EmployeeDataService { get; set; }

		[Inject]
		public ICountryDataService CountryDataService { get; set; }

		[Inject]
		public IJobCategoryDataService JobCategoryDataService { get; set; }

		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Inject]
		public ITaskDataService TaskDataService { get; set; }

		[Parameter]
		public int EmployeeId { get; set; }

		public InputText LastNameInputText { get; set; }

		public Employee Employee { get; set; } = new Employee();
		public bool ShowDialog { get; set; } = false;
		//needed to bind to select to value
		protected string CountryId = string.Empty;
		protected string JobCategoryId = string.Empty;

		//used to store state of screen
		protected string Message = string.Empty;
		protected string TaskMessage = string.Empty;
		protected string StatusClass = string.Empty;
		protected bool Saved;

		public List<Country> Countries { get; set; } = new List<Country>();
		public List<JobCategory> JobCategories { get; set; } = new List<JobCategory>();

		protected override async Task OnInitializedAsync()
		{
			Saved = false;
			Countries = (await CountryDataService.GetAllCountries()).OrderBy(o => o.Name).ToList();
			JobCategories = (await JobCategoryDataService.GetAllJobCategories()).ToList();

			if (EmployeeId == 0) //new employee is being created
			{
				//add some defaults
				Employee = new Employee { CountryId = 1, JobCategoryId = 1, BirthDate = DateTime.Now.AddYears(-18), JoinedDate = DateTime.Now.Date };
			}
			else
			{
				Employee = await EmployeeDataService.GetEmployeeDetails(EmployeeId);
			}

			CountryId = Employee.CountryId.ToString();
			JobCategoryId = Employee.JobCategoryId.ToString();
		}

		protected async Task HandleValidSubmit()
		{
			Employee.CountryId = int.Parse(CountryId);
			Employee.JobCategoryId = int.Parse(JobCategoryId);
			if (ValidateTasks() == false)
			{
				Saved = false;
				StatusClass = "alert-danger";
				TaskMessage = "There is a validation problem with the tasks please check and try again.";
				return;
			}
			if (Employee.EmployeeId == 0) //new
			{
				var addedEmployee = await EmployeeDataService.AddEmployee(Employee);
				if (addedEmployee != null)
				{
					StatusClass = "alert-success";
					Message = "New employee added successfully.";
					Saved = true;
				}
				else
				{
					StatusClass = "alert-danger";
					Message = "Something went wrong adding the new employee. Please try again.";
					Saved = false;
				}
			}
			else
			{
				await EmployeeDataService.UpdateEmployee(Employee);
				StatusClass = "alert-success";
				Message = "Employee updated successfully.";
				Saved = true;
			}
		}
		bool ValidateTasks()
		{
			foreach (var item in Employee.HRTasks)
			{
				if (string.IsNullOrEmpty(item.Title) || string.IsNullOrEmpty(item.Description))
				{
					return false;
				}
			}
			return true;
		}

		protected void HandleInvalidSubmit()
		{
			StatusClass = "alert-danger";
			Message = "There are some validation errors. Please try again.";
		}

		protected async Task DeleteEmployee()
		{
			await EmployeeDataService.DeleteEmployee(Employee.EmployeeId);

			StatusClass = "alert-success";
			Message = "Deleted successfully";
			ShowDialog = false;
			Saved = true;
		}

		protected void NavigateToOverview()
		{
			NavigationManager.NavigateTo("/employeeoverview");
		}
		protected void AddTask()
		{
			HRTask task = new HRTask { EmployeeId = Employee.EmployeeId };
			Employee.HRTasks.Add(task);
			StateHasChanged();
		}
		protected void DeleteTask(HRTask task)
		{
			Employee.HRTasks.Remove(task);
			TaskDataService.DeleteTask(task.HRTaskId);
			StateHasChanged();
		}
		protected void ShowDeleteConfirmation()
		{
			ShowDialog = true;
		}
		protected void CancelDelete()
		{
			ShowDialog = false;
		}
		private async void LookUpPostcode(string postcode)
		{
			var result = await EmployeeDataService.GetPostcodeData(postcode);
			if (result != null && result.status == 200)
			{
				Employee.Latitude = result.result.latitude;
				Employee.Longitude = result.result.longitude;
				Message = null;
			}
			else
			{
				Message = "The look up on the postcode did not return any results, please check the postcode is valid.";
				StatusClass = "alert-danger";
			}
			StateHasChanged();
		}

	}
}
