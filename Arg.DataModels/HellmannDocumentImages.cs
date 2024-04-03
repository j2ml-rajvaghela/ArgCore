using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arg.DataModels
{
    [Table("HellmannDocumentImages")]
    public class HellmannDocumentImages
    {
        [Key]
        public int ID { get; set; }
        public int? CompanyID { get; set; }
        public string Region { get; set; }
        public string RefNo { get; set; }
        public DateTime? RefDate { get; set; }
        public string Path { get; set; }
        public string fileName { get; set; }
        public DateTime? Loaded { get; set; }

        public Arg.DataModels.Enumerations.FileTypeEnum FileType
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
                        //Path = Path + fileName; dont use as it changes path if file is null
                        var file = DriveHelpers.DriveHelper.SaveFileToDrive(Path + fileName);
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
                        Path = Path.Replace(@"i:\clients\", filePath);
                    }
                    if (!string.IsNullOrWhiteSpace(fileName))
                    {
                        return Path + fileName;
                        //return "https://arg.nextpageit.com/clientdocs/pasha/sub1/" + fileName;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
