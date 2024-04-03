using Arg.DataModels;
using ArgCore.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class AddBalanceDue
    {
        public Common.CommonObjects CommonObjects = new Common.CommonObjects();

        public BOLHeader BolHeaderDetails { get; set; }
        public Arg.Agility.DataModels.BOLHeaders AgilityBolHeaderDetails { get; set; }
        public Arg.Agility.DataModels.BookingHeaders AgilityBookingHeaderDetails { get; set; }
        public Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp BookingHeaderDetails { get; set; }

        public SearchOptions SearchOptions { get; set; }
        public List<BalanceDues_Item> BalanceDuesItems { get; set; }

        public InvoiceSummary InvoiceSummary { get; set; }

        public List<BOLContainers> Containers { get; set; }
        public List<Arg.Agility.DataModels.BOLContainerDetails> AgilityContainers { get; set; }
        public List<Arg.Agility.DataModels.BOLContainerDetails> AgilityContainersSize { get; set; }
        public List<Arg.Ceva.DataAccess.BookingHeader_ContainerDetail.ContainerDetail> ContainerTypes { get; set; }

        public List<Participants> Participants { get; set; }

        public List<BOLHeader> DestinationLocList { get; set; }

        public List<BOLHeader> OriginLocList { get; set; }

        public List<BOLHeader> PolList { get; set; }

        public List<BOLHeader> PodList { get; set; }

        public List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> POD { get; set; }

        public List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> POL { get; set; }

        public List<Modes> Modes { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> ServiceMovementType { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> Origin { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> PortOfExit { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> PortOfEntry { get; set; }
        public List<Arg.Agility.DataModels.BOLHeaders> Destination { get; set; }
        public List<Arg.Ceva.DataAccess.BookingHeader.BookingHeaderImp> ModeCeva { get; set; }
        public List<Arg.Ceva.DataAccess.XrefPackagingCodes.XrefPackagingCode> TypeCeva { get; set; }

        public List<BalanceDues_OtherCharges> BalanceDuesOtherCharges { get; set; }
        public List<BOLChargesModel> PashaBalanceDuesOtherCharges { get; set; }
        public List<Arg.Agility.DataModels.SalesInvoices> AgilityBalanceDuesOtherCharges { get; set; }
        public List<BDOtherChargeCode> ChargeList { get; set; }

        //public string ErrorCode { get; set; }

        public List<Arg.DataModels.BdErrorCodes> BDErrorCodes { get; set; }

        public decimal AmountDue { get; set; }

        public decimal TotalAmountDue { get; set; }

        public decimal AmountPaid { get; set; }

        public decimal TotalCharges { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string Description { get; set; }
        public string ErrorDescription { get; set; }
        public string AdditionalDescription { get; set; }

        public string ErrorCode { get; set; }

        public string Quote { get; set; }

        public int PaymentId { get; set; }

        public string CustomerId { get; set; }

        public BalanceDue BalanceDue { get; set; }

        public List<Generic> CustomersList { get; set; }
        public List<Arg.Agility.DataModels.Generic> AgilityCustomersList { get; set; }
        public List<Arg.Ceva.DataAccess.Generic> CustList { get; set; }

        public List<BOLContainers> ContainerSizes { get; set; }
        public List<Arg.Ceva.DataAccess.BookingHeader_ContainerDetail.ContainerDetail> ContainerSizesCeva { get; set; }

        public string BolNo { get; set; }

        public string Message { get; set; }

        public string ShippersRefNo { get; set; }

        public string ConsigneeRefNo { get; set; }

        public string PayorRefNo { get; set; }

        //[AllowHtml]
        [DataType(DataType.Html)]
        public string BDDescription { get; set; }

        public SelectList BDDescriptions { get; set; }

        public string Currency { get; set; }

        public IEnumerable<SelectListItem> Currencies { get; set; }
    }
}
