using MSPApplication.Shared;
using MSPApplication.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
	public partial class CompanyDetailEdit
	{
		[Inject]
		public ICompanyDetailDataService CompanyDetailDataService { get; set; }

		[Inject]
		public ICountryDataService CountryDataService { get; set; }
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		[Parameter]
		public int CompanyDetailId { get; set; }

		public CompanyDetail CompanyDetail { get; set; } = new CompanyDetail();
		public List<Country> Countries { get; set; } = new List<Country>();
		//used to store state of screen
		protected string Message = string.Empty;
		protected string StatusClass = string.Empty;
		protected bool Saved;

		protected override async Task OnInitializedAsync()
		{
			Saved = false;
			Countries = (await CountryDataService.GetAllCountries()).OrderBy(o => o.Name).ToList();
			if (CompanyDetailId == 0) //new Company Detail is being created
			{
				//add some defaults
				CompanyDetail = new CompanyDetail() { Active = true };
			}
			else
			{
				CompanyDetail = await CompanyDetailDataService.GetCompanyDetailById(CompanyDetailId);
			}
		}
		protected async Task HandleValidSubmit()
		{
			if (CompanyDetail.Id == 0) //new
			{
				var addedCompanyDetail = await CompanyDetailDataService.AddCompanyDetail(CompanyDetail);
				if (addedCompanyDetail != null)
				{
					StatusClass = "alert-success";
					Message = "New Company Detail added successfully.";
					Saved = true;
				}
				else
				{
					StatusClass = "alert-danger";
					Message = "Something went wrong adding the new Company Detail. Please try again.";
					Saved = false;
				}
			}
			else
			{
				await CompanyDetailDataService.UpdateCompanyDetail(CompanyDetail);
				StatusClass = "alert-success";
				Message = "Company Detail updated successfully.";
				Saved = true;
			}
		}

		protected void HandleInvalidSubmit()
		{
			StatusClass = "alert-danger";
			Message = "There are some validation errors. Please try again.";
		}

		protected async Task DeleteCompanyDetail()
		{
			await CompanyDetailDataService.DeleteCompanyDetail(CompanyDetail.Id);

			StatusClass = "alert-success";
			Message = "Deleted successfully";

			Saved = true;
		}

		protected void NavigateToOverview()
		{
			NavigationManager.NavigateTo("/companydetailsoverview");
		}
	}
}
