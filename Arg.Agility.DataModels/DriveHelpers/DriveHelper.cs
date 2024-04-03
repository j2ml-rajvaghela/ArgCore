using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using Microsoft.Win32;
using System.Diagnostics;

namespace Arg.Agility.DataModels.DriveHelpers
{
    public class DriveHelper
    {
        public static DriveService _driveService = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = SharedHelper.SharedHelper.UserCredential,
            ApplicationName = SharedHelper.SharedHelper.ApplicationName
        });

        public static bool OpenReadOnlySharingForFile(string filedId)
        {
            BatchRequest.OnResponse<Permission> callback = delegate (Permission permission, RequestError error, int index, HttpResponseMessage message)
            {
                if (error != null)
                {
                    Console.WriteLine(error.Message);
                }
                else
                {
                    Console.WriteLine("Permission ID: " + permission.Id);
                }
            };
            BatchRequest batchRequest = new BatchRequest(_driveService);
            var createRequest = _driveService.Permissions.Create(new Permission
            {
                Type = "anyone",
                Role = "Reader",
                ExpirationTime = DateTime.Now.AddYears(1)
            }, filedId);

            createRequest.Fields = "id";
            batchRequest.Queue(createRequest, callback);
            batchRequest.ExecuteAsync();
            return true;
        }

        private static string GetMimeType(string fileName)
        {
            string result = "application/unknown";
            string name = Path.GetExtension(fileName).ToLower();
            RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(name);
            if (registryKey != null && registryKey.GetValue("Content Type") != null)
            {
                result = registryKey.GetValue("Content Type").ToString();
            }
            return result;
        }

        public static string SaveFileToDrive(string localFilePath)
        {
            if (System.IO.File.Exists(localFilePath))
            {
                Google.Apis.Drive.v3.Data.File file = new Google.Apis.Drive.v3.Data.File();
                file.Name = Path.GetFileName(localFilePath);
                file.Description = "File uploaded by  Drive Sample";
                file.MimeType = GetMimeType(localFilePath);
                MemoryStream stream = new MemoryStream(System.IO.File.ReadAllBytes(localFilePath));
                FilesResource.CreateMediaUpload createMediaUpload = _driveService.Files.Create(file, stream, GetMimeType(localFilePath));
                createMediaUpload.Upload();
                Google.Apis.Drive.v3.Data.File responseBody = createMediaUpload.ResponseBody;
                OpenReadOnlySharingForFile(responseBody.Id);
                return $"https://drive.google.com/file/d/{responseBody.Id}/preview?usp=drivesdk";
            }

            Trace.TraceInformation("File does not exist: " + localFilePath);
            return null;
        }
    }
}
