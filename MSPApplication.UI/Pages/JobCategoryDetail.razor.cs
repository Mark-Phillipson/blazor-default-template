using Microsoft.AspNetCore.Components;
using MSPApplication.ComponentsLibrary.Map;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class JobCategoryDetail
    {
        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Parameter]
        public int JobCategoryId { get; set; }

        public JobCategory JobCategory { get; set; } = new JobCategory();

        protected override async Task OnInitializedAsync()
        {
            JobCategory = await JobCategoryDataService.GetJobCategoryById(JobCategoryId);
        }
    }
}
