using Microsoft.AspNetCore.Components;
using MSPApplication.Shared;
using MSPApplicationDotNet6.UI.Services;
using System;
using System.Threading.Tasks;

namespace MSPApplicationDotNet6.UI.Pages
{
    public partial class NoticeEdit
    {
        [Inject]
        public INoticeDataService NoticeDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int NoticeId { get; set; }

        public Notice Notice { get; set; } = new Notice();

        //used to store state of screen
        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;
        public bool ShowDialog { get; set; } = false;
        protected override async Task OnInitializedAsync()
        {
            Saved = false;
            if (NoticeId == 0) //new Notice is being created
            {
                //add some defaults
                Notice = new Notice { DatePosted = DateTime.Now.Date, Priority = NoticePriority.Low };
            }
            else
            {
                Notice = await NoticeDataService.GetNoticeById(NoticeId);
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (Notice.NoticeId == 0) //new
            {
                var addedNotice = await NoticeDataService.AddNotice(Notice);
                if (addedNotice != null)
                {
                    StatusClass = "alert-success";
                    Message = "New Notice added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new Notice. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await NoticeDataService.UpdateNotice(Notice);
                StatusClass = "alert-success";
                Message = "Notice updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteNotice()
        {
            await NoticeDataService.DeleteNotice(Notice.NoticeId);

            StatusClass = "alert-success";
            Message = "Deleted successfully";
            ShowDialog = false;
            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/noticeoverview");
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
