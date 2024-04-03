using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("TemplateCats")]
    public class TemplateCats
    {
        [Dapper.Contrib.Extensions.Key]
        public int CatId { get; set; }

        //[Required]
        public string Name { get; set; }

        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
    }
}
