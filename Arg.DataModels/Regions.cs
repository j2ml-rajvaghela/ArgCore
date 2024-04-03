using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("Regions")]
    public class Regions
    {
        [Dapper.Contrib.Extensions.Key]
        public int RegionId { get; set; }

        [Required(ErrorMessage = "The Company field is required")]
        public int CompanyId { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Description { get; set; }

        [Computed]
        public string Company { get; set; }
    }
}
