using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPApplicationDotNet6.UI.Services;
using MSPApplicationDotNet6.UI.Components;
using MSPApplicationDotNet6.UI.Shared;


namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class JobCategoryOverview
    {
        [Inject]
        public IJobCategoryDataService jobCategoryDataService { get; set; }

        [Inject]
        public ILogger<JobCategoryOverview> Logger { get; set; }

        public List<JobCategory> JobCategories { get; set; }

        public string SearchTerm { get; set; }
#pragma warning disable 414
        private bool _loadFailed = false;
#pragma warning restore 414
        private string title = "All Job Categories";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                JobCategories = (await jobCategoryDataService.GetAllJobCategories()).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async Job Category Data Service", exception);
                _loadFailed = true;
            }
        }
        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                JobCategories = JobCategories.Where(v => v.JobCategoryName.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Job Categories With {SearchTerm} Contained within the Job Category Name";
            }
            else
            {
                JobCategories = (await jobCategoryDataService.GetAllJobCategories()).ToList();
                title = "All Job Categories";
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }

    }
}
