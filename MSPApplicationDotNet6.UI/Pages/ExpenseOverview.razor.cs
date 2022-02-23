using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MSPApplication.Shared;
using Microsoft.AspNetCore.Components;
using MSPApplicationDotNet6.UI.Services;
using MSPApplicationDotNet6.UI.Components;
using MSPApplicationDotNet6.UI.Shared;

namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class ExpenseOverview
    {
        [Inject]
        public IExpenseDataService ExpenseService { get; set; }

        public List<Expense> Expenses { get; set; }


        protected override async Task OnInitializedAsync()
        {
            Expenses = (await ExpenseService.GetAllExpenses()).ToList();
        }
    }
}
