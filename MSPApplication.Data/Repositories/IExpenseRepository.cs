using MSPApplication.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.Data.Repositories
{
    public interface IExpenseRepository
    {
        public IEnumerable<Expense> GetAllExpenses();
        public Expense GetExpenseById(int id);
        Expense UpdateExpense(Expense expense);
        Expense AddExpense(Expense expense);
        void DeleteExpense(int id);
    }
}
