using Microsoft.AspNetCore.Components;
using MSPApplication.ComponentsLibrary.Map;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class EmployeeDetail
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Inject]
        public IJobCategoryDataService JobCategoryDataService { get; set; }

        [Parameter]
        public string EmployeeId { get; set; }

        public List<Marker> MapMarkers { get; set; } = new List<Marker>();

        protected string JobCategory = string.Empty;

        public Employee Employee { get; set; } = new Employee();

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployeeDetails(int.Parse(EmployeeId));

            MapMarkers = new List<Marker>
            {
                new Marker{Description = $"{Employee.FirstName} {Employee.LastName}",  ShowPopup = false, X = Employee.Longitude, Y = Employee.Latitude}
            };
            JobCategory = (await JobCategoryDataService.GetJobCategoryById(Employee.JobCategoryId)).JobCategoryName;
        }
    }
}
