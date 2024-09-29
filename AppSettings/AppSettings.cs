using ACRPhoneWebHook.AppSettings;

namespace ACRPhone.Webhook.AppSettings
{
    public class AppSettings
    {
        public required ElmahConfig ElmahConfig { get; set; }
        public required UserCredentials UserCredentials { get; set; }
        public required string UploadPath { get; set; }
        public required string AppDataPath { get; set; }   
    }
}
