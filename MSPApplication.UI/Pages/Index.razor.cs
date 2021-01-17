using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class Index
    {
        public Concern NewConcern { get; set; } = new Concern();

        public bool EmailSent { get; set; } = false;

        [Inject]
        public IEmailService EmailSerivce { get; set; }

        [Inject]
        public ITaskDataService TaskService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<HRTask> Tasks { get; set; } = new List<HRTask>();
        [CascadingParameter]
        public ClaimsPrincipal User { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Tasks = (await TaskService.GetAllTasks()).OrderBy(o => o.Status).Take(3).ToList();
        }

        public void AddTask()
        {
            NavigationManager.NavigateTo("taskedit");
        }

        public void SubmitConcern()
        {
            var newEmail = new Email()
            {
                Body = NewConcern.Description,
                Subject = NewConcern.Title
            };

            EmailSerivce.SendEmail(newEmail);
            EmailSent = true;
        }

        public void ResetForm()
        {
            NewConcern = new Concern();
            EmailSent = false;
        }

    }
}
