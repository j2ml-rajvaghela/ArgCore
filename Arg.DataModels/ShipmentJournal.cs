using Dapper.Contrib.Extensions;

namespace Arg.DataModels
{
    [Table("ShipmentJournal")]
    public class ShipmentJournal
    {
       [Key]
        public int ID { get; set; }
        public string Region { get; set; }
        public string Issuing_Dept { get; set; }
        public string Shipment_No { get; set; }
        public DateTime Shipment_Date { get; set; }
        public string M_AWB_BL_No { get; set; }
        public string AWB_BL_No { get; set; }
        public string Shipment_Type { get; set; }
        public string Shipment_Status { get; set; }
        public string Shipper_Consignee { get; set; }
        public string Sales_Rep_Name { get; set; }
        public string Shipment_Terms { get; set; }
        public string Inco_Terms { get; set; }
        public string origin { get; set; }
        public string dest { get; set; }
        public string M_Carrier_Code { get; set; }
        public DateTime ETD { get; set; }
        public DateTime ETA { get; set; }
        public string Shipment_CL_Status { get; set; }
        public decimal? Actual_Weight_kgs { get; set; }
        public decimal? Cubic_Volume_MTQ { get; set; }
        public decimal? Chargeable_Weight { get; set; }
        public string Accrual_Existent { get; set; }
        public decimal? Actual_Revenue_CCR { get; set; }
        public decimal? Accrual_Revenue_Total_CCR { get; set; }
        public decimal? Expected_Revenue_CCR { get; set; }
        public decimal? Actual_Cost_CCR { get; set; }
        public decimal? Accrual_Cost_Total_CCR { get; set; }
        public decimal? Expected_Cost_CCR { get; set; }
        public decimal? Actual_Gross_Profit_CCR { get; set; }
        public decimal? Expected_Profit_CCR { get; set; }
        public string Username_Save_Prebooking { get; set; }
        public string Username_Save_Finishing { get; set; }
        public string Remarks { get; set; }
        public DateTime? loaded { get; set; }

        [Computed]
        public int Idx { get; set; }

        [Computed]
        public int? ResultCount { get; set; }

        [Computed]
        public string BOLViewed { get; set; }

        [Computed]
        public int? ShipmentCount { get; set; }

        [Computed]
        public string RegionDescription { get; set; }

        [Computed]
        public string ShipmentTypeDescription { get; set; }

        [Computed]
        public string ShipmentStatusDescription { get; set; }

        [Computed]
        public string ShipmentCLStatusDescription { get; set; }
    }
}
