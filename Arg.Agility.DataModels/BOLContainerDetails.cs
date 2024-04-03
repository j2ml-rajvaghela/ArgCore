using Dapper.Contrib.Extensions;

namespace Arg.Agility.DataModels
{
    [Table("BOLContainerDetails")]
    public class BOLContainerDetails
    {
        public string BranchCode { get; set; }
        public string BookingReference { get; set; }
        public string JobNumber { get; set; }
        public string ConsignmentID { get; set; }
        public string Direction { get; set; }
        public string ConsolMaster { get; set; }
        public DateTime? JobCreatedDate { get; set; }
        public DateTime? JobConfirmationDate { get; set; }
        public string ServiceLevel { get; set; }
        public string ServiceType { get; set; }
        public string ServiceMovementType { get; set; }
        public string CarrierCode { get; set; }
        public string UnitNumber { get; set; }
        public string UnitSeal { get; set; }
        public string UnitSeqNumber { get; set; }

        [Computed]
        public string UnitType { get; set; }

        public string UnitTypeDescription { get; set; }
        public int UnitMaxGrossWeight { get; set; }
        public int UnitMaxVolume { get; set; }
        public int UnitHeight { get; set; }
        public int UnitWidth { get; set; }
        public int UnitLength { get; set; }
        public int UnitDoorHeight { get; set; }
        public int UnitDoorWidth { get; set; }
        public int TempControlled { get; set; }
        public int ActUnitTaxW { get; set; }
        public int ActUnitGrossW { get; set; }
        public int ActUnitVolume { get; set; }
        public int ActUnitPieces { get; set; }

        [Computed]
        public string HazmatFlag { get; set; }

        public string TempZoneUnit { get; set; }
        public string TempZoneFrom { get; set; }
        public string TempZoneTo { get; set; }
        public string HandlingInstructions { get; set; }
        public string GoodsDescription { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string BOL { get; set; }
        public DateTime? loaded { get; set; }
        public string MD5 { get; set; }
        public string File_Name { get; set; }
        public string UnitTypeCode { get; set; }
        public string HazmatFlagCode { get; set; }
    }
}
