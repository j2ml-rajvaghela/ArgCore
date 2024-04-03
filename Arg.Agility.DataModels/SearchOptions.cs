namespace Arg.Agility.DataModels
{
    public class SearchOptions
    {
        public int CompanyId { get; set; }
        public string BillType { get; set; }
        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string unitNumber { get; set; }
        public string ImageType { get; set; }
        public string BOLViews { get; set; }
        public string DepartureStartDate { get; set; }
        public string DepartureEndDate { get; set; }
        public string JobConfirmationStartDate { get; set; }
        public string LoadStartDate { get; set; }
        public string TempFromZone { get; set; }
        public string TempToZone { get; set; }
        public string LoadEndDate { get; set; }
        public bool EliminateBDResearchItems { get; set; }
        public List<string> ShipperID { get; set; }
        public List<string> ConsigneeID { get; set; }
        public List<string> OriginLocationCode { get; set; }
        public List<string> DestinationLocationCode { get; set; }
        public string BOLNo { get; set; }
        public List<string> ServiceMovementTypeCode { get; set; }
        public List<string> ServiceLevelCode { get; set; }
        public List<string> ServiceTypeCode { get; set; }
        public List<string> PortOfExitCode { get; set; }
        public List<string> PortofEntryCode { get; set; }
        public List<string> NotifyPartyCode { get; set; }
        public List<string> ExportingCarrierCode { get; set; }
        public List<string> UnitTypeCode { get; set; }
        public List<string> HazMatFlagCode { get; set; }
        public List<string> PrepaidCollectCode { get; set; }
        public List<string> UserIds { get; set; }
        public bool ResultTableFormat { get; set; }

        public List<SortOptionsObj> SortOptions { get; set; }

        //public string ToString
        //{
        //    get
        //    {
        //        var txt = "";

        //        return txt;
        //    }
        //}
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

