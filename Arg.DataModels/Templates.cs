using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Arg.DataModels
{
    [Table("Templates")]
    public class Templates
    {
        [Dapper.Contrib.Extensions.Key]
        public int TemplateId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string EmailSubject { get; set; }

        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }

        [Required(ErrorMessage = "The Category field is required")]
        public int CatId { get; set; }

        [Required]
        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Content { get; set; }

        [Computed]
        public string CategoryName { get; set; }
    }
}
