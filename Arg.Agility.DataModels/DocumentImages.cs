using System.Diagnostics;

namespace Arg.Agility.DataModels
{
    public class DocumentImages
    {
        public int CompanyID { get; set; }
        public string JobNumber { get; set; }
        public string Path { get; set; }
        public string fileName { get; set; }
        public string Type { get; set; }
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

        public string FileUrl
        {
            get
            {
                var fileServerPath = System.Configuration.ConfigurationManager.AppSettings["FilesServerDrive"] ?? @"E:\clientdocs\";
                var filePath = System.Configuration.ConfigurationManager.AppSettings["FilePath"] ?? "https://atlas-argglobal.net/clientdocs/ClientAgility/200426/";
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
                        var file = (Path + fileName);
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
                        Path = Path.Replace(@"\\argvm2\DMS1\", filePath);
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
