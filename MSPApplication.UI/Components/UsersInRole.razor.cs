using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplication.UI.Components
{
    public partial class UsersInRole
    {
        [Parameter]
        public string RoleId { get; set; }
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public ILogger<UsersInRole> Logger { get; set; }

        public List<AspNetUser> Users { get; set; }

#pragma warning disable 414
        private bool _loadFailed = false;
        private string title = "Users";
#pragma warning restore 414

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Users = (await UserDataService.GetAllUsersInRole(RoleId)).OrderBy(v => v.UserName).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async User Data Service", exception);
                _loadFailed = true;
            }
        }
    }
}
