using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Util.Store;

namespace Arg.Agility.DataModels.SharedHelper
{
    public class SharedHelper
    {
        public static string[] Scopes = new string[2]
        {
           "https://www.googleapis.com/auth/spreadsheets",
            DriveService.Scope.Drive
        };

        public static string ApplicationName = "level-facility-222705";

        public static UserCredential _userCredential;

        public static UserCredential UserCredential
        {
            get
            {
                if (_userCredential != null)
                {
                    return _userCredential;
                }

                string text = Path.Combine(AppContext.BaseDirectory, "GoogleCredentials");
                using FileStream stream = new FileStream(Path.Combine(text, "client_secret_900617941828-2fchcv1fjbssd21mk2ceirho6t9l1n8u.apps.googleusercontent.com.json"), FileMode.Open, FileAccess.Read);
                var text2 = Path.Combine(text, "sheets.googleapis.com-dotnet-quickstart.json");
                _userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets, Scopes, "users", CancellationToken.None, new FileDataStore(text2, fullPath: true)).Result;
                Console.WriteLine("Credential file saved to: " + text2);
                return _userCredential;
            }
        }
    }
}
