using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[BalanceDues.Customers.Contacts]")]
    public class BalanceDues_Customers_Contacts
    {
        [Dapper.Contrib.Extensions.Key]
        public int ContactId { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be of 10 Digits.")]
        public string PhoneNo { get; set; }

        [Required]
        public string CustomerId { get; set; }

        public string Region { get; set; }
        public int CompanyId { get; set; }

        [Required]
        public string CustomerLocationCode { get; set; }

        [Computed]
        public string UserName
        { get { return FirstName + " " + LastName; } }
    }
}
