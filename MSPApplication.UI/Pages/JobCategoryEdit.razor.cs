using MSPApplication.Shared;
using MSPApplication.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class JobCategoryEdit
    {
        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; } = new JobCategory();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            if (JobCategoryId == 0) //new Job Category is being created
            {
                //add some defaults
                JobCategory = new JobCategory();
            }
            else
            {
                JobCategory = await JobCategoryDataService.GetJobCategoryById(JobCategoryId);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (JobCategory.JobCategoryId == 0) //new
            {
                var addedJobCategory = await JobCategoryDataService.AddJobCategory(JobCategory);
                if (addedJobCategory != null)
                {
                    StatusClass = "alert-success";
                    Message = "New Job Category added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new Job Category. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await JobCategoryDataService.UpdateJobCategory(JobCategory);
                StatusClass = "alert-success";
                Message = "Job Category updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteJobCategory()
        {
            await JobCategoryDataService.DeleteJobCategory(JobCategory.JobCategoryId);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/jobcategoryoverview");
        }
    }
}
