using Microsoft.AspNetCore.Components;
using MSPApplication.ComponentsLibrary.Map;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Pages
{
	public partial class CompanyDetailDetail
	{
		[Inject]
		public ICompanyDetailDataService CompanyDetailDataService { get; set; }
		[Inject]
		public ICountryDataService CountryDataService { get; set; }
		[Parameter]
		public int CompanyDetailId { get; set; }

		public CompanyDetail CompanyDetail { get; set; } = new CompanyDetail();
		public Country Country { get; set; }

		protected override async Task OnInitializedAsync()
		{
			CompanyDetail = await CompanyDetailDataService.GetCompanyDetailById(CompanyDetailId);
			Country = await CountryDataService.GetCountryById(CompanyDetail.CountryId);
		}
	}
}
