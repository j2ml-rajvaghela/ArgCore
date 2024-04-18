using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class BookingsImpl
    {
        public Bookings GetBookingInfo(string bolNo)
        {
            const string query = @"SELECT h.BOL#, b.BookingID,b.BookingCreateDate,b.BookingSource, b.Service,b.Direction,CONCAT(b.Voyage,' ',b.Vessel) AS Voyage,
                                   (SELECT CONCAT(b.OriginLocationCode,' ',l.CityState) From Locations l WHERE l.LocationCode=b.OriginLocationCode) AS Origin,
                                   (SELECT CONCAT(b.DestinationLocationCode,' ',l.CityState) From Locations l WHERE l.LocationCode=b.DestinationLocationCode) AS Destination,
                                   (SELECT CONCAT(b.POL,' ',l.CityState) From Locations l WHERE l.LocationCode=b.POL) AS POL,
                                   (SELECT CONCAT(b.POD,' ',l.CityState) From Locations l WHERE l.LocationCode=b.POD) AS POD,
                                   b.Mode,b.ScheduleDepartureDate,b.ActualDepartureDate,b.ShipperReferenceNumber,
                                   b.ForwarderReferenceNumber,b.ConsigneeReferenceNumber
                                   FROM Bookings b
                                   INNER JOIN BOLHeader h ON h.BookingID=b.BookingID WHERE h.BOL#=@BolNo;";

            using (var connection = Common.ClientDatabase)
            {
                var bookingInfo = connection.QueryFirstOrDefault<Bookings>(query, new { BolNo = bolNo});
                return bookingInfo;
            }
        }

        public Bookings GetBookingItemDetails(string bolNo)
        {
            const string query = @"SELECT b.BookingWeight,b.EquipmentSize,b.EquipmentType,b.BookingID, CONCAT(b.CommodityCode,' ',b.CommodityDescription) AS Commodity,
                                   b.Temperature,b.MinTemperature,b.MaxTemperature,b.CelsiusOrFahrenheit, b.UNHazmatCode,b.SITFlag,b.HaulageOrSpecialInstructions
                                   FROM Bookings b
                                   INNER JOIN BOLHeader h ON h.BookingID=b.BookingID
                                   WHERE h.BOL#=@BolNo;";

            using (var connection = Common.ClientDatabase)
            {
                var bookingItemDetail = connection.QueryFirstOrDefault<Bookings>(query, new { BolNo = bolNo });
                return bookingItemDetail;
            }
        }

        public Bookings GetBOLHeaderSection(string bolNo)
        {
            const string query = @"SELECT h.BOL# AS BOLNo,h.BillType AS BillType,h.Mode,b.Service,b.Direction,CONCAT(h.Vessel,' ',h.Voyage) AS Voyage,h.BookingID,
                                  (SELECT CONCAT(h.OriginLocationCode,' ',l.CityState) FROM Locations l WHERE LocationCode=h.OriginLocationCode) AS Origin,
                                  (SELECT CONCAT(h.POL,' ',l.CityState) FROM Locations l WHERE LocationCode=h.POL) AS POL,
                                  (SELECT CONCAT(h.POD,' ',l.CityState) FROM Locations l WHERE LocationCode=h.POD) AS POD,
                                  (SELECT CONCAT(h.DestinationLocationCode,' ',l.CityState) FROM Locations l WHERE LocationCode=h.DestinationLocationCode) AS Destination,
                                  h.ScheduleDepartureDate,h.ActualDepartureDate,h.ShipperID,h.ConsigneeID,
                                  (SELECT p.ParticipantName FROM Participants p WHERE p.ParticipantID=h.ShipperID) AS ShipperName,
                                  (SELECT p.ParticipantName FROM Participants p WHERE p.ParticipantID=h.ConsigneeID) AS ConsigneeName FROM BOLHeader h,
                                  INNER JOIN Bookings b ON h.BookingID=b.BookingID
                                  WHERE h.BOL#=@BolNo;";

            using (var connection = Common.ClientDatabase)
            {
                var bolHeaderSection = connection.QueryFirstOrDefault<Bookings>(query, new { BolNo = bolNo });
                return bolHeaderSection;
            }
        }

    }
}
