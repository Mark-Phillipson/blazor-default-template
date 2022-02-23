using System.Collections.Generic;
using System.Threading.Tasks;
using MSPApplication.Shared;

namespace MSPApplicationDotNet6.UI.Services
{
    public interface ICountryDataService
    {
        Task<IEnumerable<Country>> GetAllCountries();
        Task<Country> GetCountryById(int countryId);
    }
}
