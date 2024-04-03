using System.ComponentModel.DataAnnotations.Schema;

namespace Arg.DataModels
{
    public class BOLHazardous
    {
        [Column("Bol#")]
        public string BOLNo { get; set; }

        public string ContainerID { get; set; }
        public string MZPRIN { get; set; }
        public string Vessel { get; set; }
        public string Voyage { get; set; }
        public string MZRTIT { get; set; }
        public string AddRecord { get; set; }

        [Column("EMS#")]
        public string EMSNo { get; set; }

        public string MFAG { get; set; }
        public string CommodityDescription { get; set; }
        public string PhoneNumber { get; set; }
        public string PlacardDescription1 { get; set; }
        public string PlacardDescription2 { get; set; }
        public string PlacardDescription3 { get; set; }
        public string PlacardDescription4 { get; set; }
        public string PlacardDescription5 { get; set; }
        public string MZCODE { get; set; }
        public string HazmatFlag { get; set; }
        public string CommodityDescription2 { get; set; }
        public string MZHCON { get; set; }
        public string MZLABL { get; set; }
        public string MZLBL2 { get; set; }
        public string MZLBL3 { get; set; }
        public string MZIMOL { get; set; }
        public string PlacardNumber { get; set; }
        public string MZPAGE { get; set; }
        public string UNHazmatCode { get; set; }
        public string MZPKGG { get; set; }
        public string MZFLPT { get; set; }
        public string MZMPOL { get; set; }
        public string MZEMSL { get; set; }
        public string MZEMRG { get; set; }
        public string MZNPKS { get; set; }
        public string MZPKTP { get; set; }
        public string MZIPKG { get; set; }
        public string MZOPKG { get; set; }
        public decimal? Weight { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? Measure { get; set; }
        public string MZIORM { get; set; }
        public string Contact { get; set; }
        public string MZDECR { get; set; }
        public string MZSTAT { get; set; }
        public string MZHAUT { get; set; }
        public string MZHAPV { get; set; }
        public string MZSUBR { get; set; }
        public string MZDOTL { get; set; }
        public string MZDOTD { get; set; }
        public string MZSLGT { get; set; }
        public string MZIMOT { get; set; }
        public string MZSTUD { get; set; }
        public string MZPRST { get; set; }
        public string MZREM1 { get; set; }
        public string MZREM2 { get; set; }
        public int? Sequence { get; set; }
        public int? Quantity { get; set; }
        public string MZNEXQ { get; set; }
        public string MZSUBD { get; set; }
        public string MZPDES { get; set; }
        public string ProperShippingName { get; set; }
        public string MZFLD1 { get; set; }
        public string MZFLD2 { get; set; }
        public string MZFLD3 { get; set; }
        public string MZFLD4 { get; set; }
        public DateTime? Uploaded { get; set; }
    }
}
