using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface ICurrencyRepository
    {
        IEnumerable<Currency> GetAllCurrencys();

        Currency GetCurrencyById(int id);

        Currency AddCurrency(Currency Currency);
    }
}