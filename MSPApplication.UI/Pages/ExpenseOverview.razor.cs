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
