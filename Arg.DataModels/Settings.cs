using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("Settings")]
    public class Settings
    {
        [Dapper.Contrib.Extensions.Key]
        public int SettingId { get; set; }

        [Required(ErrorMessage = "The Group field is required")]
        public int GroupId { get; set; }

        [Required]
        public string Label { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
