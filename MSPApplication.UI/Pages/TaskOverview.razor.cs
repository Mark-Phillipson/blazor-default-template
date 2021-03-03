using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplication.UI.Components;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
	public partial class TaskOverview
	{
		[Inject] public ITaskDataService TaskDataService { get; set; }
		[Inject]public ILogger<TaskOverview> Logger { get; set; }
		[Inject] NavigationManager NavigationManager { get; set; }
		[CascadingParameter] public IModalService Modal { get; set; }
		[Inject] public IJSRuntime JSRuntime { get; set; }
		public List<HRTask> Tasks { get; set; }
		public List<HRTask> FilteredTasks { get; set; }
		private string searchTerm;
		public string SearchTerm { get => searchTerm; set { searchTerm = value; ApplyFilter(); } }
#pragma warning disable 414, 649
		private bool _loadFailed = false;
		ElementReference SearchInput;
#pragma warning restore 414, 649
		private string title = "All Tasks";
		protected override async Task OnInitializedAsync()
		{
			await LoadData();
		}

		private async Task LoadData()
		{
			try
			{
				Tasks = (await TaskDataService.GetAllTasks()).ToList();
			}
			catch (Exception exception)
			{
				Logger.LogError("Exception occurred in on initialised async Task Data Service", exception);
				_loadFailed = true;
			}
			FilteredTasks = Tasks;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (firstRender)
			{
				await SearchInput.FocusAsync();
			}
		}
		[CascadingParameter] ClaimsPrincipal User { get; set; }

		protected async Task AddNewTaskAsync()
		{
			var parameters = new ModalParameters();
			parameters.Add(nameof(User), User);
			//parameters.Add(nameof(ParentId), ParentId);
			var formModal = Modal.Show<AddTask>("Add Task", parameters);
			var result = await formModal.Result;
			if (!result.Cancelled)
			{
				await LoadData();
			}
		}

		private void ApplyFilter()
		{
			if (!string.IsNullOrEmpty(SearchTerm))
			{
				FilteredTasks = FilteredTasks.Where(v => v.Title.ToLower().Contains(SearchTerm.Trim().ToLower()) || v.Description.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
				title = $"Tasks With {SearchTerm} Contained within the Title/description";
			}
			else
			{
				FilteredTasks = Tasks;
				title = "All Tasks";
			}
		}
		private async Task CallChangeAsync(string elementId)
		{
			await JSRuntime.InvokeVoidAsync("CallChange", elementId);
			ApplyFilter();
		}
		private void EditTask(int id)
		{
			NavigationManager.NavigateTo($"taskedit/{id}");
		}

	}
}
