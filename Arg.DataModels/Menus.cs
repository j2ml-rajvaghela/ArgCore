using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("Menus")]
    public class Menus
    {
        [Dapper.Contrib.Extensions.Key]
        public int MenuId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Display Name field is required")]
        public string DisplayName { get; set; }

        public DateTime AddedOn { get; set; }
        public DateTime LastModOn { get; set; }
        public int AddedBy { get; set; }
        public int LastModBy { get; set; }
    }
}
