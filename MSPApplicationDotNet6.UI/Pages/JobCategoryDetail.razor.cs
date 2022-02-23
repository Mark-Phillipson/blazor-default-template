using Microsoft.AspNetCore.Components;
using MSPApplication.ComponentsLibrary.Map;
using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSPApplicationDotNet6.UI.Services;
using MSPApplicationDotNet6.UI.Components;
using MSPApplicationDotNet6.UI.Shared;

namespace MSPApplicationDotNet6.UI.Pages
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
