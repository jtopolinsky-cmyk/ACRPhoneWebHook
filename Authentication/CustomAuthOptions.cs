using Microsoft.AspNetCore.Authentication;

namespace ACRPhoneWebHook.Authentication
{
    public class CustomAuthOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "custom auth";
        public string Scheme => DefaultScheme;
        public KeyValuePair<string, string> UsernameKeyValue { get; set; }
        public KeyValuePair<string, string> PasswordKeyValue { get; set; }
    }
}