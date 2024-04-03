using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("[BalanceDues.Item]")]
    public class BalanceDues_Item
    {
        [Key]
        public int ItemId { get; set; }
        public int CompanyId { get; set; }
        public string CustomerId { get; set; }
        public string Region { get; set; }
        public string CustomerLocationCode { get; set; }
        public string BookingId { get; set; }
        public string Bol { get; set; }
        public string BalanceDueInvoice { get; set; }
        public DateTime BalanceDueInvoiceDate { get; set; }
        public string TariffRef { get; set; }
        public string Commodity { get; set; }
        public string CommodityDesc { get; set; }
        public bool Hazmat { get; set; }
        public bool LtdQty { get; set; }
        public string Type { get; set; }
        public string Container { get; set; }
        public int CIDNo { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerType { get; set; }
        public string Weight { get; set; }
        public string WeightUnit { get; set; }
        public string CBF { get; set; }
        public string CVT { get; set; }
        public string CBM { get; set; }
        public string Measure { get; set; }
        public string MeasureUnit { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        //public string I/M { get; set; }
        public int Quantity { get; set; }

        public string PackageType { get; set; }

        //public string T/S { get; set; }
        public string Opt { get; set; }

        public string Seal { get; set; }
        public string Ref { get; set; }
        public string Remark { get; set; }
        public string SIT { get; set; }
        public string Set { get; set; }
        public string CF { get; set; }
        public string NonOperating { get; set; }
        public string Goods { get; set; }
        public string CargoGlass { get; set; }
        public int Rate { get; set; }
        public decimal AmountDue { get; set; }

        [Computed]
        public string ChargeableWeight { get; set; }

        public string GrossWeight { get; set; }
    }
}
