using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("[BalanceDues.Customers]")]
    public class Customers
    {

        [Dapper.Contrib.Extensions.Key]
        public int BdCustomerId { get; set; }
        public string CustomerId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        //[Required(ErrorMessage = "The Customer Location Code field is required")]
        //public string CustomerLocationCode { get; set; }
        [Required]
        public string Region { get; set; }

        [Required(ErrorMessage = "The Customer Name field is required")]
        public string CustomerName { get; set; }

        //[Required(ErrorMessage = "The Contact Title field is required")]
        //public string ContactTitle { get; set; }
        //[Required(ErrorMessage = "The Contact Name field is required")]
        //public string ContactName { get; set; }
        //[Required(ErrorMessage = "The Phone No. field is required")]
        //[DataType(DataType.PhoneNumber)]
        //[StringLength(10, MinimumLength = 10)]
        //public string ContactPhone { get; set; }
        //[Required(ErrorMessage = "The Contact Email field is required")]
        //public string ContactEmail { get; set; }
        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        public DateTime LastUpdated { get; set; }

        [Computed]
        public string Company { get; set; }

        [Computed]
        public string Customer { get; set; }
    }
}
