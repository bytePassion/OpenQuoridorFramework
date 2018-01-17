using System;
using System.Windows.Media;

namespace bytePassion.OnkoTePla.Resources
{
    public static class GlobalConstants
    {		
        public static class TcpIpPort
        {						 
            public const uint Request   = 6657;
            public const uint PubSub    = 6658;	            		
        }

        public static readonly Color[] RoomColors =
        {
            Colors.White,
            Color.FromRgb( 33,150,243), 
            Color.FromRgb(  3,169,244),
            Color.FromRgb(  0,188,212),  
            Color.FromRgb(  0,150,136),  
            Color.FromRgb( 76,175, 80),  
            Color.FromRgb(139,195, 74),  
            Color.FromRgb(205,220, 57),  
            Color.FromRgb(255,235, 59),  
            Color.FromRgb(255,193,  7),  
            Color.FromRgb(255,152,  0)  
        };
        
        public static readonly TimeSpan ResponseWaitingTime = TimeSpan.FromSeconds(5);
        public static readonly TimeSpan SendingTimeout      = TimeSpan.FromSeconds(2);
        public static readonly TimeSpan HeartbeatIntverval  = TimeSpan.FromSeconds(5);

        private static readonly string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        
        public static readonly string ServerBasePath       = AppDataPath + @"\bytePassion\OnkoTePla\Server\";
        public static readonly string EventHistoryBasePath = ServerBasePath + "EventHistory";

        public const string DataFileExtension = ".otpj";
        
        public static readonly string PatientPersistenceFile          = ServerBasePath + "patients"  + DataFileExtension;		
        public static readonly string UserPersistenceFile             = ServerBasePath + "user"      + DataFileExtension;				
        public static readonly string LabelPersistenceFile            = ServerBasePath + "label"     + DataFileExtension;				
        public static readonly string TherapyPlaceTypePersistenceFile = ServerBasePath + "placeType" + DataFileExtension;				
        public static readonly string MedicalPracticePersistenceFile  = ServerBasePath + "practice"  + DataFileExtension;				
        public static readonly string MetaDataPersistanceFile         = ServerBasePath + "metaData"  + DataFileExtension;
        public static readonly string SettingsPersistanceFile         = ServerBasePath + "settings"  + DataFileExtension;        
    }
}
