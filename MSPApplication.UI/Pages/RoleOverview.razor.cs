using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class RoleOverview
    {
        [Inject]
        public IRoleDataService RoleDataService { get; set; }

        [Inject]
        public ILogger<RoleOverview> Logger { get; set; }

        public List<AspNetRole> Roles { get; set; }

        public string SearchTerm { get; set; }
#pragma warning disable 414,649
        private bool _loadFailed = false;
        ElementReference SearchInput;
#pragma warning restore 414,649
        public string ExceptionMessage { get; set; }
        private string title = "All Roles";
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Roles = (await RoleDataService.GetAllRoles()).OrderBy(v => v.Name).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async Role Data Service", exception);
                ExceptionMessage = exception.Message;
                _loadFailed = true;
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("myJsFunctions.focusElement", SearchInput);
            }
        }
        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Roles = Roles.Where(v => v.Name.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Roles With {SearchTerm} Contained within the Name";
            }
            else
            {
                Roles = (await RoleDataService.GetAllRoles()).ToList();
                title = "All Roles";
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }

    }
}
