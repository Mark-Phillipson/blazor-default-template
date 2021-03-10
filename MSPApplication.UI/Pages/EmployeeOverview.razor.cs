using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplication.UI.Components;
using MSPApplication.UI.Services;
using MSPApplication.UI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
	public partial class EmployeeOverview
	{
		[Inject] public IEmployeeDataService EmployeeDataService { get; set; }
		[Inject] NavigationManager NavigationManager { get; set; }
		[Inject] public ILogger<EmployeeOverview> Logger { get; set; }
		[Inject] public IToastService ToastService { get; set; }
		[CascadingParameter] public IModalService Modal { get; set; }
		[Parameter] public JobCategory JobCategory { get; set; }
		[CascadingParameter] public ClaimsPrincipal User { get; set; }
		public string Title { get; set; } = "All Employees";
		private string searchTerm;
		public string SearchTerm { get => searchTerm; set { searchTerm = value; ApplyFilter(); } }

		public List<Employee> Employees { get; set; }
		public List<Employee> FilteredEmployees { get; set; }
		protected AddEmployee AddEmployee { get; set; }
#pragma warning disable 414, 649
		private bool _loadFailed = false;
		ElementReference SearchInput;
#pragma warning restore 414, 649
		public string ExceptionMessage { get; set; } = String.Empty;
		protected override async Task OnInitializedAsync()
		{
			await LoadData();
		}

		async Task LoadData()
		{
			try
			{
				Employees = (await EmployeeDataService.GetAllEmployees(JobCategory?.JobCategoryId)).ToList();
				if (JobCategory != null)
				{
					Title = $"{JobCategory.JobCategoryName} Employees";
				}
			}
			catch (Exception e)
			{
				Logger.LogError("Exception occurred in on initialised async Employee Data Service", e);
				_loadFailed = true;
				ExceptionMessage = e.Message;
				ToastService.ShowError(e.Message, "Error Loading Employee");
			}
			FilteredEmployees = Employees;

		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSRuntime.InvokeVoidAsync("myJsFunctions.focusElement", SearchInput);
			}
		}
		protected async Task AddNewEmployeeAsync()
		{
			var parameters = new ModalParameters();
			parameters.Add(nameof(User), User);
			//parameters.Add(nameof(ParentId), ParentId);
			var formModal = Modal.Show<AddEmployee>("Add Employee", parameters);
			var result = await formModal.Result;
			if (!result.Cancelled)
			{
				await LoadData();
			}
		}
		private void ApplyFilter()
		{
			if (string.IsNullOrEmpty(SearchTerm))
			{
				FilteredEmployees = Employees.OrderBy(v => v.FullName).ToList();
				Title = $"All Employees ({FilteredEmployees.Count})";
			}
			else
			{
				FilteredEmployees = Employees.Where(v => v.FullName.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
				Title = $"Filtered Employees ({FilteredEmployees.Count})";
			}
		}
		protected void SortEmployees(string sortColumn)
		{
			if (sortColumn == "Job Title")
			{
				FilteredEmployees = FilteredEmployees.OrderBy(o => o.JobCategory.JobCategoryName).ThenBy(t => t.FullName).ToList();
			}
			else if (sortColumn == "FullName")
			{
				FilteredEmployees = FilteredEmployees.OrderBy(v => v.FullName).ToList();
			}
			else if (sortColumn == "FullName Desc")
			{
				FilteredEmployees = FilteredEmployees.OrderByDescending(v => v.FullName).ToList();
			}
			else if (sortColumn == "Email")
			{
				FilteredEmployees = FilteredEmployees.OrderBy(v => v.Email).ToList();
			}
			else if (sortColumn == "EmployeeId")
			{
				FilteredEmployees = FilteredEmployees.OrderBy(v => v.EmployeeId).ToList();
			}
		}
		async Task DeleteEmployeeAsync(int employeeId)
		{
			var parameters = new ModalParameters();
			parameters.Add("Title", "Please Confirm");
			parameters.Add("Message", "Do you really wish to delete this Employee?");
			parameters.Add("ButtonColour", "danger");
			var employee = await EmployeeDataService.GetEmployeeDetails(employeeId);
			var formModal = Modal.Show<BlazoredModalConfirmDialog>($"Delete Employee: {employee?.FullName}?", parameters);
			var result = await formModal.Result;
			if (!result.Cancelled)
			{
				try
				{
					await EmployeeDataService.DeleteEmployee(employeeId);
				}
				catch (Exception)
				{
					throw;
				}
				ToastService.ShowSuccess($"{employee.FullName} Successfully deleted.", "Delete Employee");
				await LoadData();
			}
		}
		void DetailEmployeeAsync(int employeeId)
		{
			NavigationManager.NavigateTo($"employeedetail/{employeeId}");
		}
		void EditEmployeeAsync(int employeeId)
		{
			NavigationManager.NavigateTo($"employeeedit/{employeeId}");
		}
	}
}
