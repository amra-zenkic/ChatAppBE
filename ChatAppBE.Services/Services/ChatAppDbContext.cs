using ChatAppBE.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBE.Services.Services
{
    public class ChatAppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public ChatAppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>();
            modelBuilder.Entity<Message>();
        }
    }
}
