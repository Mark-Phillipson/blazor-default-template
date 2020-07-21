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
    public class StaffDirectoryBase: ComponentBase
    {
        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        public List<Employee> Employees { get; set; }

        protected AddEmployeeDialog AddEmployeeDialog { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetAllEmployees()).ToList();
        }
    }
}
