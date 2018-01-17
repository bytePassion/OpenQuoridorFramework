using System.Collections.Generic;

namespace bytePassion.OnkoTePla.Resources
{
    public static class ApplicationInfo
    {
        public static readonly IEnumerable<string> AvailableLanguageCodes = new List<string>
        {
            "de",
            "en"
        };

        public const string ClientVersion     = "3.2 Beta";
        public const string ServerVersion     = "3.2 Beta";
        public const string BackupToolVersion = "1.0 Beta";
    }
}
