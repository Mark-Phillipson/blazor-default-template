using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MSPApplication.Shared;
using MSPApplication.UI.Components;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class EmployeeOverview
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public ILogger<EmployeeOverview> Logger { get; set; }

        [Parameter]
        public JobCategory JobCategory { get; set; }

        public string Title { get; set; } = "All Employees";
        public string SearchTerm { get; set; } = null;

        public List<Employee> Employees { get; set; }

        protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Employees = (await EmployeeDataService.GetAllEmployees(JobCategory?.JobCategoryId)).ToList();
                if (JobCategory != null)
                {
                    Title = $"{JobCategory.JobCategoryName} Employees";
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Exception occurred in on initialised async Employee Data Service", e);
            }
        }

        public async void AddEmployeeDialog_OnDialogClose()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
            StateHasChanged();
        }

        protected void QuickAddEmployee()
        {
            AddEmployeeDialog.Show();
        }
    }
}
