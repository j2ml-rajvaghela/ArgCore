using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    [Dapper.Contrib.Extensions.Table("BOLCharges")]
    public class BOLChargesModel
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }
        public int? Sequence { get; set; }
        public string ChargeCode { get; set; }
        public string ChargeDescription { get; set; }
        public string MBWTMS { get; set; }
        public decimal? ChargeRate { get; set; }
        public string MBUOFM { get; set; }
        public string PrepaidorCollect { get; set; }
        public decimal? USAmount { get; set; }
        public decimal? LocalAmount { get; set; }
        public string MBEXRT { get; set; }
        public string Currency { get; set; }
        public string Tariff { get; set; }
        public decimal? MBSCHB { get; set; }
        public string MBRTIT { get; set; }
        public string MBCCOL { get; set; }
        public string PayorID { get; set; }
        public string CorrectedBOL { get; set; }
        public DateTime? Uploaded { get; set; }

        [Computed]
        public string Charge { get; set; }

        [Computed]
        public string Payor { get; set; }

        [Computed]
        public string ChargeCodeValue { get; set; }
    }
}
