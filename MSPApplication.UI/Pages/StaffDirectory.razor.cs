using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MSPApplication.UI.Components;
using MSPApplication.UI.Services;
using MSPApplication.Shared;
using Microsoft.AspNetCore.Components;

namespace MSPApplication.UI.Pages
{
    public partial class StaffDirectory
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public List<Employee> Employees { get; set; }

        protected AddEmployee AddEmployeeDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).OrderBy(v => v.FirstName).ThenBy(t => t.LastName).ToList();
        }
    }
}
