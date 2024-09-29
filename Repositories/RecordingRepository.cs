using ACRPhoneWebHook.Models;

namespace ACRPhoneWebHook.Repositories
{
    public class RecordingRepository(WebhookContext dbContext) : RepositoryBase<Recording, long>(dbContext), IRecordingRepository
    {
    }
}
