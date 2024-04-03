using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("MenuItems")]
    public class MenuItems
    {
        [Dapper.Contrib.Extensions.Key]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "The Menu field is required")]
        public int MenuId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Display Name field is required")]
        public string DisplayName { get; set; }

        [Required]
        public string Link { get; set; }

        [Required(ErrorMessage = "The Icon field is required")]
        public string ImgSrc { get; set; }

        public DateTime AddedOn { get; set; }
        public DateTime LastModOn { get; set; }
        public int AddedBy { get; set; }
        public int LastModBy { get; set; }
        public int ParentId { get; set; }

        [Required(ErrorMessage = "The Display Index field is required")]
        public int DisplayIdx { get; set; }

        [Computed]
        public string MenuItemName { get; set; }

        [Computed]
        public string MenuItemDisName { get; set; }
    }
}
