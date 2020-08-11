using MSPApplication.Shared;
using System.Collections.Generic;

namespace MSPApplication.Data.Repositories
{
    public interface ICompanyDetailRepository
    {
        IEnumerable<CompanyDetail> GetAllCompanyDetails();
        CompanyDetail GetCompanyDetailById(int id);
        CompanyDetail AddCompanyDetail(CompanyDetail companyDetail);
        CompanyDetail UpdateCompanyDetail(CompanyDetail companyDetail);
        void DeleteCompanyDetail(int id);
    }
}
