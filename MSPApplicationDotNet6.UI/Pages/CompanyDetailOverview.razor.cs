using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Components;
using MSPApplicationDotNet6.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class CompanyDetailOverview
    {
        [Inject]
        public ICompanyDetailDataService CompanyDetailDataService { get; set; }

        [Inject]
        public ILogger<CompanyDetailOverview> Logger { get; set; }

        public string Title { get; set; } = "Company Details";
        public string SearchTerm { get; set; } = null;

        public List<CompanyDetail> CompanyDetails { get; set; }

#pragma warning disable 414, 649
        private bool _loadFailed = false;
        ElementReference SearchInput;
#pragma warning restore 414, 649
        public string ExceptionMessage { get; set; } = String.Empty;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                CompanyDetails= (await CompanyDetailDataService.GetAllCompanyDetails()).ToList();
            }
            catch (Exception e)
            {
                Logger.LogError("Exception occurred in on initialised async Company Detail Data Service", e);
                _loadFailed = true;
                ExceptionMessage = e.Message;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                 await SearchInput.FocusAsync();
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }
        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                CompanyDetails= CompanyDetails.Where(v => v.CompanyName.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                Title = $"Company Details with {SearchTerm} Contained within the Company Name";
            }
            else
            {
                CompanyDetails= (await CompanyDetailDataService.GetAllCompanyDetails()).ToList();
                Title = "All Employees";
            }
        }
        protected void SortCompanyDetails(string sortColumn)
        {
            if (sortColumn == "CompanyName")
            {
                CompanyDetails = CompanyDetails.OrderBy(v => v.CompanyName).ToList();
            }
        }
    }
}
