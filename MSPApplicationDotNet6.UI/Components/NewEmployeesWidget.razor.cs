using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSPApplicationDotNet6.UI.Services;
using MSPApplication.Shared;
using Microsoft.AspNetCore.Components;

namespace MSPApplicationDotNet6.UI.Components
{
    public partial class NewEmployeesWidget
    {
        public List<Employee> NewEmployees { get; set; } = new List<Employee>();

        [Inject]
        public IEmployeeDataService EmployeeDataService { get; set; }

        [Parameter] public int HowManyToReturn { get; set; } = 3;

        protected override async Task OnInitializedAsync()
        {
            NewEmployees = (await EmployeeDataService.GetAllEmployees()).OrderByDescending(x => x.JoinedDate).Take(HowManyToReturn).ToList();
        }
    }
}
