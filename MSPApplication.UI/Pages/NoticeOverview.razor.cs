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
    public partial class NoticeOverview
    {
        [Inject]
        public INoticeDataService noticeDataService { get; set; }

        [Inject]
        public ILogger<NoticeOverview> Logger { get; set; }

        public List<Notice> Notices { get; set; }

        public string SearchTerm { get; set; }
#pragma warning disable 414
        private bool _loadFailed = false;
#pragma warning restore 414
        private string title = "All Notices";

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Notices = (await noticeDataService.GetAllNotices()).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async Notice Data Service", exception);
                _loadFailed = true;
            }
        }
        private async Task ApplyFilter()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Notices = Notices.Where(v => v.Description.ToLower().Contains(SearchTerm.Trim().ToLower())).ToList();
                title = $"Notices With {SearchTerm} Contained within the Notice Description";
            }
            else
            {
                Notices = (await noticeDataService.GetAllNotices()).ToList();
                title = "All Notices";
            }
        }
        private async Task CallChangeAsync(string elementId)
        {
            await JSRuntime.InvokeVoidAsync("CallChange", elementId);
            await ApplyFilter();
        }

    }
}
