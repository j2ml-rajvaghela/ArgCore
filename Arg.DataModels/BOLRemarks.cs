using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLRemarks")]
    public class BOLRemarks
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }

        public int? Sequence { get; set; }
        public string Remarks { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
