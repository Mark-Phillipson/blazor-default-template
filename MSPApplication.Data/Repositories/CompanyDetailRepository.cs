using MSPApplication.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MSPApplication.Data.Repositories
{
    public class CompanyDetailRepository : ICompanyDetailRepository
    {
        private readonly AppDbContext _appDbContext;

        public CompanyDetailRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<CompanyDetail> GetAllCompanyDetails()
        {
            return _appDbContext.CompanyDetails;
        }

        public CompanyDetail GetCompanyDetailById(int id)
        {
            return _appDbContext.CompanyDetails.FirstOrDefault(c => c.Id == id);
        }

        public CompanyDetail AddCompanyDetail(CompanyDetail companyDetail)
        {
            var addedEntity = _appDbContext.CompanyDetails.Add(companyDetail);
            _appDbContext.SaveChanges();
            return addedEntity.Entity;
        }

        public CompanyDetail UpdateCompanyDetail(CompanyDetail companyDetail)
        {
            var foundCompanyDetail = _appDbContext.CompanyDetails.FirstOrDefault(e => e.Id == companyDetail.Id);

            if (foundCompanyDetail != null)
            {
                foundCompanyDetail.CountryId = companyDetail.CountryId;
                foundCompanyDetail.AddressLine1 = companyDetail.AddressLine1;
                foundCompanyDetail.AddressLine2 = companyDetail.AddressLine2;
                foundCompanyDetail.Active = companyDetail.Active;
                foundCompanyDetail.CompanyName = companyDetail.CompanyName;
                foundCompanyDetail.City = companyDetail.City;
                foundCompanyDetail.EmailAddress = companyDetail.EmailAddress;
                foundCompanyDetail.PhoneNumber = companyDetail.PhoneNumber;
                foundCompanyDetail.Postcode = companyDetail.Postcode;
                foundCompanyDetail.StateProvinceCounty = companyDetail.StateProvinceCounty;
                foundCompanyDetail.WebAddress = companyDetail.WebAddress;

                _appDbContext.SaveChanges();

                return foundCompanyDetail;
            }
            return null;
        }

        public void DeleteCompanyDetail(int id)
        {
            var foundCompanyDetail = _appDbContext.CompanyDetails.FirstOrDefault(e => e.Id == id);
            if (foundCompanyDetail == null) return;

            _appDbContext.CompanyDetails.Remove(foundCompanyDetail);
            _appDbContext.SaveChanges();
        }
    }
}
