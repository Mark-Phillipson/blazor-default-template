using MSPApplication.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplication.UI.Services
{
    public interface ICompanyDetailDataService
    {
        Task<CompanyDetail> AddCompanyDetail(CompanyDetail companyDetail);
        Task DeleteCompanyDetail(int id);
        Task<IEnumerable<CompanyDetail>> GetAllCompanyDetails();
        Task<CompanyDetail> GetCompanyDetailById(int id);
        Task UpdateCompanyDetail(CompanyDetail companyDetail);
    }
}
