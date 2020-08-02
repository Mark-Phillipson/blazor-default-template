using System;
using System.Collections.Generic;

namespace MSPApplication.Shared
{
    public partial class AspNetUserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public string UserId { get; set; }

        public AspNetUser User { get; set; }
    }
}
