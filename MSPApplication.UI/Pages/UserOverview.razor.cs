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
    public partial class UserOverview
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<UserOverview> Logger { get; set; }

        public List<AspNetUser> Users { get; set; }

        public string SearchTerm { get; set; }
#pragma warning disable 414,649
        private bool _loadFailed = false;
        ElementReference SearchInput;
#pragma warning restore 414,649
        public string ExceptionMessage { get; set; }
        private string title = "All Users";
        protected override async Task OnInitializedAsync()
        {
            try
            {
                Users = (await UserDataService.GetAllUsers()).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async User Data Service", exception);
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
                Users = Users.Where(v => v.UserName.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Users With {SearchTerm} Contained within the Username";
            }
            else
            {
                Users = (await UserDataService.GetAllUsers()).ToList();
                title = "All Users";
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }

    }
}
