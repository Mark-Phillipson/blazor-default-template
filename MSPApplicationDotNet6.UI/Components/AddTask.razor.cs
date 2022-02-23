using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Components
{
	public partial class AddTask
	{
		[CascadingParameter] BlazoredModalInstance ModalInstance { get; set; }
		[Parameter] public ClaimsPrincipal User { get; set; }
		[Inject] public IToastService ToastService { get; set; }
		[Inject] public IJSRuntime JSRuntime { get; set; }
		public HRTask Task { get; set; } = new HRTask();
		[Inject] public ITaskDataService TaskDataService { get; set; }
		[Inject] public IEmployeeDataService EmployeeDataService { get; set; }
		public List<Employee> Employees { get; set; } = new List<Employee>();
		protected string EmployeeId =  null;
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSRuntime.InvokeVoidAsync("window.setFocus", "title");
			}
		}
		protected override async Task OnInitializedAsync()
		{
			Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
			Task = new HRTask { Status = HRTaskStatus.Open };

			var employee = Employees.FirstOrDefault(e => e.Email.ToLower() == User?.Identity?.Name?.ToLower());
			Task.EmployeeId = employee?.EmployeeId;
		}

		public void Close()
		{
			ModalInstance.CancelAsync();
		}

		protected async Task HandleValidSubmit()
		{
			await TaskDataService.AddTask(Task);
			await ModalInstance.CloseAsync(ModalResult.Ok(true));
			ToastService.ShowSuccess($"{Task.Title} Added successfully.", "Success");
		}
	}
}
