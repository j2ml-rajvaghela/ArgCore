namespace Arg.Ceva.DataAccess
{
    public class SearchOptions
    {
        public int ClientId { get; set; }
        public int CompanyId { get; set; }
        public List<string> Region { get; set; }
        public List<string> Mode { get; set; }
        public List<string> Branch { get; set; }
        public string AWBNO { get; set; }
        public string Bol { get; set; }

        public string HAWBNO { get; set; }
        public string HBLNO { get; set; }
        public string HOUSENO { get; set; }
        public List<string> CNTRYCODE { get; set; }
        public List<string> CNTRYCOD01 { get; set; }
        public List<string> POL { get; set; }
        public List<string> POD { get; set; }
        public string EstimatedStartDate { get; set; }
        public string EstimatedEndDate { get; set; }
        public List<string> ONCSERV { get; set; }
        public bool ResultTableFormat { get; set; }
        public List<SortOptionsObj> SortOptions { get; set; }
        public List<string> CSORNO { get; set; }
        public List<string> CSEENO { get; set; }
        public List<string> SHPRNO { get; set; }
        public List<string> PCKGSCODE { get; set; }
        public List<string> CARRCODE { get; set; }
        public List<string> CARR { get; set; }
        public List<string> SERVLEVEL { get; set; }
        public List<string> SERVLVL { get; set; }
        public List<string> SERVTYPE { get; set; }
        public List<string> TYPE { get; set; }
        public List<string> ImageType { get; set; }

        public decimal BookingMaxPieceCount { get; set; }
        public decimal BookingMinPieceCount { get; set; }
        public decimal NetRateMaxAmount { get; set; }
        public decimal NetRateMinAmount { get; set; }
        public decimal BookingMinActualWeight { get; set; }
        public decimal BookingMaxActualWeight { get; set; }
        public decimal BookingMinMeasure { get; set; }
        public decimal BookingMaxMeasure { get; set; }
        public decimal BookingMinChargeableWeight { get; set; }
        public decimal BookingMaxChargeableWeight { get; set; }
        public string GoodsDescOperator { get; set; }
        public string BookingRemarksOperator { get; set; }
        public string GoodsDescription { get; set; }
        public string BookingRemarks { get; set; }
        public string HandlingRemarks { get; set; }
        public string HandlingRemarksOperator { get; set; }
        public string ChargeDesc { get; set; }

        public string ChargeDescOperator { get; set; }

        public string ChargeMinAmount { get; set; }
        public string ChargeMaxAmount { get; set; }
        public string ContainerId { get; set; }
        public string ContainerIdOperator { get; set; }
        public string BOLViews { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> CNTRTYPE { get; set; }
    }

    public class SortOptionsObj
    {
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public int Idx { get; set; }
        public bool IsDesc { get; set; }
        public bool IsSelected { get; set; }
    }
}
