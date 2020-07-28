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
        private ITaskDataService taskDataService { get; set; }

        [Inject]
        private NavigationManager navigationManager { get; set; }

        [Inject]
        public IEmployeeDataService employeeDataService { get; set; }

        public List<Employee> Employees { get; set; } = new List<Employee>();

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (HRTaskId > 0)
            {
                try
                {
                    Task = (await taskDataService.GetTaskById(HRTaskId));
                }
                catch (System.Exception exception)
                {
                    Message = exception.Message;
                }
            }
            else
            {
                Task = new HRTask();
            }
            Employees = (await employeeDataService.GetAllEmployees()).ToList();
        }

        protected async Task HandleValidSubmit()
        {
            Task.AssignedTo = int.Parse(EmployeeId);
            if (Task.HRTaskId > 0)
            {
                await taskDataService.UpdateTask(Task);
                navigationManager.NavigateTo("/");
            }
            else
            {
                var result = await taskDataService.AddTask(Task);
                if (result != null)
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    Message = "An error has occured";
                }
            }

        }

    }
}
