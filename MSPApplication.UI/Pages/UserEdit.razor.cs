using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using MSPApplication.UI.Services;
using System;
using System.Threading.Tasks;

namespace MSPApplication.UI.Pages
{
    public partial class UserEdit
    {
        [Inject]
        public IUserDataService UserDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string id { get; set; }

        public AspNetUser User { get; set; } = new AspNetUser();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        public bool ShowDialog { get; set; } = false;
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
    }
}
