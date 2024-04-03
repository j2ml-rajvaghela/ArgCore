using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("IPAddressRestriction")]
    public class IPAddressRestriction
    {
        [Dapper.Contrib.Extensions.Key]
        public int IPAddressRestrictionId { get; set; }

        [Required(ErrorMessage = "The Client field is required")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Begining IP Address is required.")]
        public string BeginningIp { get; set; }

        [Required(ErrorMessage = "Ending IP Address is required.")]
        public string EndingIp { get; set; }
    }
}
