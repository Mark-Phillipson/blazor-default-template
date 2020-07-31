using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class TaskEdit
    {
        [Parameter] public int HRTaskId { get; set; } = 0;
        public bool Saved { get; set; } = false;

        public HRTask Task { get; set; } = new HRTask();

        public string Message { get; set; }

        protected string EmployeeId = "1";

        [Inject]
        private ITaskDataService TaskDataService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();
        public bool ShowDialog { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            Saved = false;
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

        protected async Task HandleValidSubmit()
        {
            if (Task.HRTaskId > 0)
            {
                await TaskDataService.UpdateTask(Task);
                NavigationManager.NavigateTo("/");
            }
            else
            {
                var result = await TaskDataService.AddTask(Task);
                if (result != null)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    Message = "An error has occured";
                }
            }
        }
        protected async Task DeleteTaskAsync()
        {
            await TaskDataService.DeleteTask(Task.HRTaskId);
            ShowDialog = false;
            Saved = true;
            Message = "The Task has been deleted successfully.";
        }
        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/tasksoverview");
        }

        protected void ShowDeleteConfirmation()
        {
            ShowDialog = true;
        }
        protected void CancelDelete()
        {
            ShowDialog = false;
        }

    }
}
