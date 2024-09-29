using Microsoft.Extensions.DependencyInjection;
using ACRPhoneWebHook.Repositories;

namespace ACRPhoneWebHook.Configuration
{
    public static class ApplicationServiceConfiguration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRecordingRepository, RecordingRepository>();
        }
    }
}
