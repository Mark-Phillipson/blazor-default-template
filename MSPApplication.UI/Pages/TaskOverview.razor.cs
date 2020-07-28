using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
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
            }
        }

    }
}
