namespace ChatAppBE.Services.Services
{
    using ChatAppBE.Models.Models;
    using Microsoft.EntityFrameworkCore;

    public class ChatAppDbContext : DbContext
    {
        public ChatAppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Message>();
        }
    }
}
