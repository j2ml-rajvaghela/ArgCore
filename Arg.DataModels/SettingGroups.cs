using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("SettingGroups")]
    public class SettingGroups
    {
        [Dapper.Contrib.Extensions.Key]
        public int GroupId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
