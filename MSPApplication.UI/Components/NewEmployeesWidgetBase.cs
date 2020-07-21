using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPApplication.UI.Services;
using MSPApplication.Shared;
using Microsoft.AspNetCore.Components;

namespace MSPApplication.UI.Components
{
    public class NewEmployeesWidgetBase : ComponentBase
    {
        public List<Employee> NewEmployees { get; set; } = new List<Employee>();

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NewEmployees = (await EmployeeDataService.GetAllEmployees()).OrderBy(x => x.JoinedDate).Take(3).ToList();
        }
    }
}
