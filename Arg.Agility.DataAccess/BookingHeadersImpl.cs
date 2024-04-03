using Arg.Agility.DataModels;
using Arg.DataAccess;
using Dapper;
using System.Data;

namespace Arg.Agility.DataAccess
{
    public class BookingHeadersImpl
    {
        public BookingHeaders GetBookingInfo(string jobNumber)
        {
            const string query = @"SELECT * FROM BookingHeaders 
                                   WHERE JobNumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var bookingInfo = connection.QueryFirstOrDefault<BookingHeaders>(query, new { JobNumber = jobNumber });
                return bookingInfo;
            }
        }

        public BookingHeaders GetConsigneeReference(string jobNumber)
        {

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(jobNumber))
            {
                parameters.Add("@JobNumber", jobNumber, DbType.String);
            }
            const string query = @"SELECT DISTINCT b.ConsignmentID FROM BookingHeaders b 
                                   WHERE JobNumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var consigneeReference = connection.QueryFirstOrDefault<BookingHeaders>(query, parameters);
                return consigneeReference;
            }
        }

        public List<BOLHeaders> GetPortOfExit()
        {
            const string query = @"SELECT DISTINCT b.PortOfExit + ' ' + ISNULL(c.LocationName, '') AS PortOfExit , b.PortOfExit AS PortOfExitCode FROM BookingHeaders b
                                   LEFT JOIN LocationCodes c ON b.PortOfExit = c.LocationCode
                                   WHERE b.PortOfExit IS NOT NULL
                                   ORDER BY PortOfExit;";

            using (var connection = Common.ClientDatabase)
            {
                var portOfExits = connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
                return portOfExits;
            }
        }

        public List<BOLHeaders> GetPortofEntry()
        {
            const string query = @"SELECT DISTINCT b.PortofEntry + ' ' + ISNULL(c.LocationName, '') AS PortofEntry , b.PortofEntry AS PortofEntryCode FROM BookingHeaders b
                                   LEFT JOIN LocationCodes c ON b.PortOfEntry = c.LocationCode
                                   WHERE b.PortOfEntry IS NOT NULL
                                   ORDER BY PortOfEntry;";

            using (var connection = Common.ClientDatabase)
            {
                var portOfEntries = connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
                return portOfEntries;
            }
        }

        public List<BOLHeaders> GetServiceMovementType()
        {
            const string query = @"SELECT DISTINCT b.ServiceMovementType , b.ServiceMovementType as ServiceMovementTypeCode FROM BookingHeaders b
                                   WHERE ServiceMovementType <> 'null'
                                   ORDER BY ServiceMovementType;";

            using (var connection = Common.ClientDatabase)
            {
                var serviceMovementTypes = connection.Query<BOLHeaders>(query, commandType: CommandType.Text).ToList();
                return serviceMovementTypes;
            }
        }

        public string GetCarrierName(string carrierCode)
        {
            const string query = @"SELECT CarrierName FROM Carriers 
                                   WHERE CarrierCode=@CarrierCode;";

            using (var connection = Common.ClientDatabase)
            {
                var carrierName = connection.QueryFirstOrDefault<string>(query, new { CarrierCode = carrierCode });
                return carrierName;
            }
        }

        public BOLHeaders GetBOLHeaderSection(string jobNumber)
        {
            const string query = @"SELECT h.loaded AS LoadDate,
                                   bh.Shipper,bh.Consignee,
                                   bh.ExportingCarrier,bh.Pieces,
                                   bh.GrossWeight,bh.ChargeableWeight,
                                   bh.Origin,bh.Destination,
                                   bh.PrepaidCollect,bh.FreightRevenue, 
                                   bh.OtherRevenue, bh.FreightCost,h.* FROM BookingHeaders h
                                   LEFT JOIN BOLHeaders bh ON bh.JobNumber = h.JobNumber 
                                   WHERE h.JobNumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var bolHeaderSection = connection.QueryFirstOrDefault<BOLHeaders>(query, new { JobNumber = jobNumber });
                return bolHeaderSection;
            }
        }

        public ShipmentTrackingDetails GetShipmentTrackingDetails(string jobNumber)
        {
            const string query = @"SELECT * FROM ShipmentTrackingDetails h
                                   WHERE h.jobnumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var trackingDetails = connection.QueryFirstOrDefault<ShipmentTrackingDetails>(query, new { JobNumber = jobNumber });
                return trackingDetails;
            }
        }

        public List<BOLContainerDetails> GetBOLContainerDetails(string jobNumber)
        {
            const string query = @"SELECT DISTINCT UnitSeqNumber,
                                   unitnumber AS  UnitNumber,
                                   unittype AS UnitType,
                                   ActUnitPieces AS ActUnitPieces,
                                   tempzonefrom AS  TempZoneFrom,
                                   handlinginstructions AS HandlingInstructions,
                                   actunitgrossw AS  ActUnitGrossW,
                                   goodsdescription AS GoodsDescription,
                                   unittypedescription AS UnitTypeDescription,
                                   tempzoneto AS TempZoneTo,
                                   actunitpieces AS ActUnitPieces,
                                   hazmatflag AS HazmatFlag,
                                   actunittaxw AS ActUnitTaxW FROM BOLContainerDetails h
                                   WHERE h.jobnumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var containerDetails = connection.Query<BOLContainerDetails>(query, new { @JobNumber = jobNumber }).ToList();
                return containerDetails;
            }
        }

        public List<SalesInvoices> GetSalesInvoices(string jobNumber)
        {
            const string query = @"SELECT ClientID , ClientIDName , InvoiceNumber , InvoiceDate,ChargeDescription,ChargeValue AS ChargeValue , InvoiceCurrency FROM SalesInvoices
                                   WHERE JobNumber=@JobNumber AND UNION ALL SELECT ClientID, ClientIDName, InvoiceNumber, InvoiceDate,ChargeDescription, 
                                   chargevalue AS ChargeValue, InvoiceCurrency
                                   FROM bulkinvoices a WHERE jobnumber=@JobNumber
                                   ORDER BY ClientID, InvoiceNumber, InvoiceDate,ClientIDName,InvoiceCurrency;";

            using (var connection = Common.ClientDatabase)
            {
                var salesInvoices = connection.Query<SalesInvoices>(query, new { @JobNumber = jobNumber }).ToList();
                return salesInvoices;
            }
        }

        public List<PurchaseInvoices> GetSupplierInvoicing(string jobNumber)
        {
            const string query = @"SELECT DISTINCT SupplierID,InvoiceNumber,InvoiceDate,ChargeCodeDescription, BaseChargeValue AS InvoiceAmount,CurrencyCode, JobNumber 
                                   FROM PurchaseInvoices h
                                   WHERE h.JobNumber=@JobNumber
                                   ORDER BY SupplierID, InvoiceNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var supplierInvoicing= connection.Query<PurchaseInvoices>(query, new { @JobNumber  = jobNumber }).ToList();
                return supplierInvoicing;
            }
        }

        public List<DocumentImages> GetDocumentImage(string jobNumber)
        {
            const string query = @"SELECT JobNumber,[Path],[FileName], [Type] FROM DocumentImages h
                                   WHERE h.JobNumber=@JobNumber
                                   ORDER BY [Type];";

            using (var connection = Common.ClientDatabase)
            {
                var documentImages = connection.Query<DocumentImages>(query, new { @JobNumber = jobNumber }).ToList();
                return documentImages;
            }
        }

        public string GetTeriffRef(string jobNumber)
        {
            const string query = @"SELECT ContractNo FROM BookingHeaders b
                                   WHERE b.JobNumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var teriffRef = connection.QueryFirstOrDefault<string>(query, new { @JobNumber = jobNumber});
                return teriffRef;
            }
        }

        public decimal GetAmountDue(string jobNumber)
        {
            const string query = @"SELECT ChargeValue FROM SalesInvoices s
                                   WHERE s.ChargeCode = 'M1A' AND s.JobNumber=@JobNumber;";

            using (var connection = Common.ClientDatabase)
            {
                var amountDue = connection.QueryFirstOrDefault<decimal>(query, new { @JobNumber = jobNumber});
                return amountDue;
            }
        }
    }
}
