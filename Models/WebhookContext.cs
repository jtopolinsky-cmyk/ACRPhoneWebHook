using Microsoft.EntityFrameworkCore;

namespace ACRPhone.Webhook.Models
{
    public class WebhookContext(DbContextOptions<WebhookContext> options) : DbContext(options)
    {
        public DbSet<Recording> Recordings { get; set; }

    }
}
