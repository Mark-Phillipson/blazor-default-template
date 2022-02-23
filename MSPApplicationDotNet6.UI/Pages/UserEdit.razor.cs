using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class UserEdit
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }
        [Inject]
        public IRoleDataService RoleDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string id { get; set; }

        public AspNetUser User { get; set; } = new AspNetUser();
        public string RoleId { get; set; }
        public List<AspNetRole> Roles { get; set; }
        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        public bool ShowDialog { get; set; } = false;
        public string RoleMessage { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (string.IsNullOrEmpty(id)) //new User is being created
            {
                //add some defaults
                User = new AspNetUser { };
            }
            else
            {
                User = await UserDataService.GetUserById(id);
            }
            Roles = (await RoleDataService.GetAllRoles()).OrderBy(v => v.Name).ToList();
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(User.Id)) //new
            {
                var addedUser = await UserDataService.AddUser(User);
                if (addedUser != null)
                {
                    StatusClass = "alert-success";
                    Message = "New User added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new User. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await UserDataService.UpdateUser(User);
                StatusClass = "alert-success";
                Message = "User updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteUser()
        {
            await UserDataService.DeleteUser(User.Id);

            StatusClass = "alert-success";
            Message = "Deleted successfully";
            ShowDialog = false;
            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/useroverview");
        }
        protected void ShowDeleteConfirmation()
        {
            ShowDialog = true;
        }
        protected void CancelDelete()
        {
            ShowDialog = false;
        }
        protected async Task AddUserRoleAsync()
        {
            var item = User.AspNetUserRoles.FirstOrDefault(e => e.RoleId == RoleId);
            if (item !=  null)
            {
                RoleMessage = "Failed to add a new role, cannot be duplicated.";
                return;
            }
            if (!string.IsNullOrEmpty(RoleId))
            {
                AspNetRole role = (await RoleDataService.GetRoleById(RoleId));
                AspNetUserRole aspNetUserRole = new AspNetUserRole { UserId = User.Id, RoleId = RoleId, Role = role };
                User.AspNetUserRoles.Add(aspNetUserRole);
            }
            else
            {
                RoleMessage = "Please select a role first before adding!";
            }
        }
        protected async Task DeleteUserRole(string userId, string roleId)
        {
            var item = User.AspNetUserRoles.Where(v => v.UserId == userId && v.RoleId == roleId).FirstOrDefault();
            User.AspNetUserRoles.Remove(item);
            await UserDataService.DeleteUserRole(userId, roleId);
        }
    }
}
