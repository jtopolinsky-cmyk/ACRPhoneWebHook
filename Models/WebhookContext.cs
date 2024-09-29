using Microsoft.EntityFrameworkCore;

namespace ACRPhoneWebHook.Models
{
    public class WebhookContext(DbContextOptions<WebhookContext> options) : DbContext(options)
    {
        public DbSet<Recording> Recordings { get; set; }

    }
}
