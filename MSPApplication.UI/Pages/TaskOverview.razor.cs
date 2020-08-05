using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class TaskOverview
    {
        [Inject]
        public ITaskDataService TaskDataService { get; set; }

        [Inject]
        public ILogger<TaskOverview> Logger { get; set; }

        public List<HRTask> Tasks { get; set; }
        public string SearchTerm { get; set; }
#pragma warning disable 414,649
        private bool _loadFailed = false;
        ElementReference SearchInput;
#pragma warning restore 414,649
        private string title = "All Tasks";

        //protected AddTaskDialog AddTaskDialog { get; set; }
        protected override async Task OnInitializedAsync()
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
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("window.myJsFunctions.focusElement", SearchInput);
            }
        }
        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Tasks = Tasks.Where(v => v.Title.ToLower().Contains(SearchTerm.Trim().ToLower()) || v.Description.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Tasks With {SearchTerm} Contained within the Title/description";
            }
            else
            {
                Tasks = (await TaskDataService.GetAllTasks()).ToList();
                title = "All Tasks";
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }

    }
}
