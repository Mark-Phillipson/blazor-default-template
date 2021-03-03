﻿using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using MSPApplication.UI.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace MSPApplication.UI.Pages
{
	public partial class TaskEdit
	{
		[Inject] public IJSRuntime JSRuntime { get; set; }
		[Inject] IToastService ToastService { get; set; }
		[Parameter] public int HRTaskId { get; set; } = 0;
		public HRTask Task { get; set; } = new HRTask();
		public string Message { get; set; }

		protected string EmployeeId = "1";

		[Inject] private ITaskDataService TaskDataService { get; set; }
		[Inject] private NavigationManager NavigationManager { get; set; }
		[Inject] public IEmployeeDataService EmployeeDataService { get; set; }
		public List<Employee> Employees { get; set; } = new List<Employee>();
		protected override async Task OnInitializedAsync()
		{
			if (HRTaskId > 0)
			{
				try
				{
					Task = (await TaskDataService.GetTaskById(HRTaskId));
				}
				catch (System.Exception exception)
				{
					Message = exception.Message;
				}
			}
			else
			{
				Task = new HRTask { Status = HRTaskStatus.Open };
			}
			Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
		}
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await JSRuntime.InvokeVoidAsync("window.setFocus", "title");
			}
		}
		protected async Task HandleValidSubmit()
		{
			if (Task.HRTaskId > 0)
			{
				await TaskDataService.UpdateTask(Task);
				ToastService.ShowSuccess("Task updated successfully", "SUCCESS");
				NavigationManager.NavigateTo("/tasksoverview/");
			}
			else
			{
				var result = await TaskDataService.AddTask(Task);
				if (result != null)
				{
					NavigationManager.NavigateTo("/tasksoverview/");
					ToastService.ShowSuccess("Task added successfully", "SUCCESS");
				}
				else
				{
					ToastService.ShowError("Error adding Task", "ERROR");
				}
			}
		}
		[CascadingParameter] public IModalService Modal { get; set; }
		async Task DeleteEmployeeAsync(int taskId)
		{
			var parameters = new ModalParameters();
			parameters.Add("Title", "Please Confirm");
			parameters.Add("Message", "Do you really wish to delete this Task?");
			parameters.Add("ButtonColour", "danger");
			var task = await TaskDataService.GetTaskById(taskId);
			var formModal = Modal.Show<BlazoredModalConfirmDialog>($"Delete Employee: {task?.Title}?", parameters);
			var result = await formModal.Result;
			if (!result.Cancelled)
			{
				await TaskDataService.DeleteTask(taskId);
				NavigateToOverview();
			}
		}
		protected void NavigateToOverview()
		{
			NavigationManager.NavigateTo("/tasksoverview");
		}
	}
}
