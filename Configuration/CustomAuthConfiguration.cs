using ACRPhoneWebHook.AppSettings;
using ACRPhoneWebHook.Authentication;
using ACRPhoneWebHook.Configuration;

namespace ACRPhoneWebHook.Configuration
{
    public static class CustomAuthConfiguration
    {
        public static void AddCustomAuth(this IServiceCollection services, AppSettings.AppSettings appSettings)
        {
            // Add authentication 
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CustomAuthOptions.DefaultScheme;
                options.DefaultChallengeScheme = CustomAuthOptions.DefaultScheme;
            })
            // Call custom authentication extension method
            .AddCustomAuth(options =>
            {
                // Configure username and password for authentication
                options.UsernameKeyValue = new KeyValuePair<string, string>("username", appSettings.UserCredentials.Username);
                options.PasswordKeyValue = new KeyValuePair<string, string>("password", appSettings.UserCredentials.Password);
            });
        }
    }
}
