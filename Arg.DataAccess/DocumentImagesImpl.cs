using Arg.DataModels;
using Dapper;

namespace Arg.DataAccess
{
    public class DocumentImagesImpl
    {
        public static string[] PriorityFiles = { "Shipping Instructions", "Dangerous Cargo Manifest", "Dangerous Cargo Paperwork" };

        public List<DocumentImages> GetDocumentImage(string bolNo, string bookingId)
        {
            const string query = @"SELECT * FROM DocumentImages i
                                   WHERE i.fileName <> '' 
                                   AND (BOL#=@BolNo OR BookingID=@BookingId)
                                   ORDER BY i.Type, i.ScanDate DESC;";

            using (var connection = Common.ClientDatabase)
            {
                var documentImages = connection.Query<DocumentImages>(query, new { @BolNo = bolNo, @BookingId = bookingId }).ToList();
                var files = new List<DocumentImages>();
                var pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[0]));
                files.AddRange(pf);
                pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[1]));
                files.AddRange(pf);
                pf = documentImages.Where(x => x.Type.Contains(PriorityFiles[2]));
                files.AddRange(pf);
                var remaining = documentImages.Except(files);
                files.AddRange(remaining);
                return files;
            }
        }
    }
}
