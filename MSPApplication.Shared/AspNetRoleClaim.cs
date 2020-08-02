using System;
using System.Collections.Generic;

namespace MSPApplication.Shared
{
    public partial class AspNetRoleClaim
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }

        public AspNetRole Role { get; set; }
    }
}
