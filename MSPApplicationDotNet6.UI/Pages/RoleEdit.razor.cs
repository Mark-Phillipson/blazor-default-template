using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class RoleEdit
    {
        [Inject]
        public IRoleDataService RoleDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string id { get; set; }

        public AspNetRole Role { get; set; } = new AspNetRole();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        public bool ShowDialog { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (string.IsNullOrEmpty(id)) //new Role is being created
            {
                //add some defaults
                Role = new AspNetRole { };
            }
            else
            {
                Role = await RoleDataService.GetRoleById(id);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(Role.Id)) //new
            {
                var addedRole = await RoleDataService.AddRole(Role);
                if (addedRole != null)
                {
                    StatusClass = "alert-success";
                    Message = "New Role added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new Role. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await RoleDataService.UpdateRole(Role);
                StatusClass = "alert-success";
                Message = "Role updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteRole()
        {
            await RoleDataService.DeleteRole(Role.Id);

            StatusClass = "alert-success";
            Message = "Deleted successfully";
            ShowDialog = false;
            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/roleoverview");
        }
        protected void ShowDeleteConfirmation()
        {
            ShowDialog = true;
        }
        protected void CancelDelete()
        {
            ShowDialog = false;
        }
    }
}
