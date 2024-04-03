using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ArgCore.Models
{
    public class BOLAuditing
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public SearchOptions SearchOptions { get; set; }
        public Arg.Ceva.DataAccess.SearchOptions SearchOptionsCeva { get; set; }

        //public Arg.Agility.DataModels.SearchOptions SearchOptionsAgility { get; set; }
        public SelectList Modes { get; set; }

        public SelectList Shippers { get; set; }

        public SelectList Consignee { get; set; }
        public SelectList Customer { get; set; }
        public SelectList Shipper { get; set; }
        public SelectList PackageType { get; set; }
        public SelectList OceanCarrier { get; set; }

        public SelectList AirCarrier { get; set; }
        public SelectList AirServiceLevels { get; set; }
        public SelectList AirServiceLeveldetail { get; set; }
        public SelectList AirServiceType { get; set; }
        public SelectList ContainerDetails { get; set; }
        public SelectList ImagedTypes { get; set; }

        public SelectList Booking { get; set; }

        public SelectList Payors { get; set; }

        public List<Arg.DataModels.BOLHeader> BOLAuditingResults { get; set; }

        public SelectList PortOfDischarge { get; set; }

        public SelectList PortOfLoading { get; set; }

        public string Message { get; set; }

        public SelectList EquipmentSizes { get; set; }

        public SelectList EquipmentTypes { get; set; }

        public SelectList UNHazmatCodes { get; set; }

        public SelectList ReferenceTypes { get; set; }

        public SelectList Origins { get; set; }

        public SelectList Destinations { get; set; }

        public IEnumerable<SelectListItem> Operators { get; set; }

        public IEnumerable<SelectListItem> SortOptions { get; set; }

        public SelectList Users { get; set; }

        public SelectList ContainerEventTypes { get; set; }

        public SelectList Companies { get; set; }

        public SelectList Regions { get; set; }
        public SelectList Mode { get; set; }
        public SelectList Branch { get; set; }
        public SelectList OriginCountryCode { get; set; }
        public SelectList DestinationCountryCode { get; set; }
        public SelectList OceanServiceType { get; set; }
        public SelectList POL { get; set; }
        public int CompanyId { get; set; }
        public string InvoiceType { get; set; }
        public List<Arg.Ceva.DataAccess.BookingHeader> BookingHeaderAuditingResults { get; set; }

        public SelectList ServiceMovementType { get; set; }
        public SelectList ServiceLevel { get; set; }
        public SelectList ServiceType { get; set; }
        public SelectList PortOfExit { get; set; }
        public SelectList PortofEntry { get; set; }
        public SelectList NotifyParty { get; set; }
        public SelectList ExportingCarrier { get; set; }
        public SelectList UnitType { get; set; }
        public SelectList HazMatFlag { get; set; }
        public SelectList PrepaidCollect { get; set; }

        public SelectList ServiceMovementTypeCode { get; set; }
        public SelectList ServiceLevelCode { get; set; }
        public SelectList ServiceTypeCode { get; set; }
        public SelectList PortOfExitCode { get; set; }
        public SelectList PortofEntryCode { get; set; }
        public SelectList NotifyPartyCode { get; set; }
        public SelectList ExportingCarrierCode { get; set; }
        public SelectList UnitTypeCode { get; set; }
        public SelectList HazMatFlagCode { get; set; }
        public SelectList PrepaidCollectCode { get; set; }
        public SelectList ShipmentTypes { get; set; }
        public SelectList ShipmentStatuss { get; set; }
        public SelectList ShipmentCLStatuss { get; set; }
        public SelectList ShipmentRegions { get; set; }
        public SelectList IssuingDepts { get; set; }
        public SelectList FilterSign { get; set; }
    }
}
