using Arg.DataAccess;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace Arg.Ceva.DataAccess
{
    public class DocumentImages
    {
        private readonly SqlConnection _connection;

        public DocumentImages()
        {
            _connection = Common.ClientDatabase;
        }

        [Table("DocumentImages")]
        public class DocumentImage
        {
            public string Region { get; set; }
            public string HAWBBLNO { get; set; }
            public string Path { get; set; }
            public string fileName { get; set; }
            public string Type { get; set; }
            public DateTime? ScanDate { get; set; }
            public DateTime? UpdatedDate { get; set; }

            public Enumerations.FileTypeEnum FileType
            {
                get
                {
                    var ext = System.IO.Path.GetExtension(fileName).ToLower();
                    if (ext == ".pdf")
                    {
                        return Enumerations.FileTypeEnum.PDF;
                    }
                    else if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        return Enumerations.FileTypeEnum.Image;
                    }
                    else if (ext == ".msg")
                    {
                        return Enumerations.FileTypeEnum.Msg;
                    }
                    else if (ext == ".docx" || ext == ".doc")
                    {
                        return Enumerations.FileTypeEnum.Document;
                    }
                    else if (ext == ".xls" || ext == ".xlsx")
                    {
                        return Enumerations.FileTypeEnum.Excel;
                    }
                    else
                    {
                        return Enumerations.FileTypeEnum.Other;
                    }
                }
            }

            [Computed]
            public string FileUrl
            {
                get
                {
                    var fileServerPath = System.Configuration.ConfigurationManager.AppSettings["FilesServerDrive"] ?? @"E:\clientdocs\";
                    var filePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"] ?? "https://atlas-argglobal.net/clientdocs/";
                    if (FileType == Enumerations.FileTypeEnum.Msg)
                    {
                        if (!string.IsNullOrWhiteSpace(Path))
                        {
                            Path = Path.Replace(@"i:\clients\", fileServerPath);
                        }
                        return Path + fileName;
                    }
                    else if (FileType == Enumerations.FileTypeEnum.Excel || FileType == Enumerations.FileTypeEnum.Document)
                    {
                        if (!string.IsNullOrWhiteSpace(Path))
                        {
                            Path = Path.Replace(@"i:\clients\", fileServerPath);
                            Trace.TraceInformation("Path: " + Path);
                            Path = Path + fileName; // dont use as it changes path if file is null
                            var file = Drive.SaveFileToDrive(Path + fileName);
                            if (string.IsNullOrWhiteSpace(file))
                            {
                                Path = Path.Replace(fileServerPath, filePath);
                                Trace.TraceInformation("Path if file is empty: " + Path);
                                return Path + fileName;
                            }
                            Trace.TraceInformation("File: " + file);
                            return file;
                        }
                        return Path + fileName;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Path))
                        {
                            Path = Path.ToLower().Replace(@"i:\clients\", filePath);
                        }
                        if (!string.IsNullOrWhiteSpace(fileName))
                        {
                            fileName = Uri.EscapeDataString(fileName);
                            return Path + "/" + fileName;
                            // return "https://arg.nextpageit.com/clientdocs/pasha/sub1/" + fileName;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }


        public static string[] PriorityFiles = { "House Bill of Lading", "OH - CEVA Customer Invoice", "IH - CEVA Customer Invoice" };

        public List<DocumentImage> GetDocumentImage(string bolNo)
        {
            const string query = @"SELECT * 
                                   FROM DocumentImages i
                                   WHERE i.fileName <> '' AND i.HAWBBLNO=@HAWBBLNO
                                   ORDER BY i.Type,i.ScanDate;";

            var documentImages = _connection.Query<DocumentImage>(query, new { HAWBBLNO = bolNo }).ToList();
            var files = new List<DocumentImage>();
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

        public List<DocumentImage> GetAllDocumentImage()
        {
            const string query = @"SELECT distinct Type 
                                   FROM DocumentImages
                                   WHERE fileName <> ''
                                   ORDER BY Type;";

            return _connection.Query<DocumentImage>(query, commandType: CommandType.Text).ToList();
        }
    }
}
