using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLContainers")]
    public class BOLContainers
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }
        public string ContainerID { get; set; }
        public int? Item { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public decimal? Weight { get; set; }
        public DateTime? Uploaded { get; set; }
        public string Container10Digit { get; set; }
    }
}
