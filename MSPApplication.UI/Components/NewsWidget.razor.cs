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
    public partial class NewsWidget
    {
        [Inject]
        public INoticeDataService noticeDataService { get; set; }

        [Inject]
        public ILogger<NewsWidget> Logger { get; set; }

        public List<Notice> Notices { get; set; }

#pragma warning disable 414
        private bool _loadFailed = false;
        private string title = "Notices";
#pragma warning restore 414

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Notices = (await noticeDataService.GetAllNotices()).Where(v => v.Show == true).ToList();
            }
            catch (Exception exception)
            {
                Logger.LogError("Exception occurred in on initialised async Notice Data Service", exception);
                _loadFailed = true;
            }
        }
    }
}
