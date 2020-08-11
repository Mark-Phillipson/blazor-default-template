using System.ComponentModel.DataAnnotations;

namespace MSPApplication.Shared
{
    public class CompanyDetail
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(255)]
        public string AddressLine1 { get; set; }
        [StringLength(255)]
        public string AddressLine2 { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(40)]
        public string StateProvinceCounty { get; set; }
        [StringLength(50)]
        public string Postcode { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        [Url]
        [StringLength(100)]
        public string WebAddress { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
    }
}
