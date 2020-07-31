using System;
using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public class Notice
    {
        public int NoticeId { get; set; }
        [StringLength(500)]
        [Required]
        public string Description { get; set; }
        public NoticePriority Priority { get; set; }
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }
        public bool Show { get; set; }
    }
}
