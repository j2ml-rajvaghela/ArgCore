using System.ComponentModel.DataAnnotations;
namespace Arg.DataModels
{
    public class ArgClient
    {
        [Dapper.Contrib.Extensions.Key]
        public int CompanyId { get; set; }
        public int InvoiceTerms { get; set; }
        public decimal UnderBillingCommissionRate { get; set; }
        public string CompanyInfo { get; set; }

        [Required]
        public string ImportDataPath { get; set; }

        [Required]
        public string DBName { get; set; }

        public DateTime LastAccessDate { get; set; }

        [Required]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [StringLength(12, MinimumLength = 10)]
        public string Fax { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        //[Required(ErrorMessage = "The Phone No. field is required")]
        [StringLength(12, MinimumLength = 10)]
        public string Contact { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string CollectorFirstName { get; set; }

        [Required]
        public string CollectorLastName { get; set; }

        [Required]
        public string CollectorEmail { get; set; }

        public bool BolBilltype { get; set; }
        public decimal OverchargeFee { get; set; }
    }
}
