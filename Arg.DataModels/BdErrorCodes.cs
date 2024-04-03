using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("BdErrorCodes")]
    public class BdErrorCodes
    {
        [Dapper.Contrib.Extensions.Key]
        public int ErrorCodeId { get; set; }
        public int CompanyId { get; set; }

        [Required]
        public string BdErrorCode { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Computed]
        public string ErrorCodes { get; set; }

        [Computed]
        public string Company { get; set; }
    }
}
