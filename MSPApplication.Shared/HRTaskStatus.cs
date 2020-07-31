using System;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    [Flags]
    public enum HRTaskStatus
    {
        [Display(Name = "Open")]
        Open,
        [Display(Name = "Assigned")]
        Assigned,
        [Display(Name = "In Progress")]
        InProgress,
        [Display(Name = "Blocked")]
        Blocked,
        [Display(Name = "Complete")]
        Complete,
        [Display(Name = "Backburner")]
        Backburner
    }
}
