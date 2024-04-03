using CacheManager.Core;

namespace Arg.Core
{
    public class Settings
    {
        public static string FilePath =>  System.Configuration.ConfigurationManager.AppSettings["FilePath"] ?? "https://atlas-argglobal.net/clientdocs/";

        public static string FilesServerDrive => System.Configuration.ConfigurationManager.AppSettings["FilesServerDrive"] ?? @"I:\\clients\";// will not work on local.. local >> E:\clientdocs

        public static Action<ConfigurationBuilderCachePart> DefaultCacheSettings
        {
            get
            {
                return p => p.WithDictionaryHandle();
            }
        }
    }
}
