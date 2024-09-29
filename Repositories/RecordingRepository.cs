using ACRPhone.Webhook.Models;

namespace ACRPhone.Webhook.Repositories
{
    public class RecordingRepository(WebhookContext dbContext) : RepositoryBase<Recording, long>(dbContext), IRecordingRepository
    {
    }
}
