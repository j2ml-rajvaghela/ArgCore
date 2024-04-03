using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLReference")]
    public class BOLReference
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }

        [Column("MasterBOL#")]
        public string MasterBOLNo { get; set; }

        public string ReferenceType { get; set; }
        public string MRQUAL { get; set; }
        public string AddRecord { get; set; }
        public string Reference { get; set; }
        public string User { get; set; }
        public string ReferenceDate { get; set; }
        public string ReferenceTime { get; set; }
        public int? Sequence { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
