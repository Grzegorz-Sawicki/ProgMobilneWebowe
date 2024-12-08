using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace L6Backend.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<Chat> Chats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.ChatGroups)
                .WithMany(cg => cg.Users);

            modelBuilder.Entity<ChatGroup>()
                .HasMany(cg => cg.Users)
                .WithMany(u => u.ChatGroups);

            modelBuilder.Entity<ChatGroup>()
                .HasOne(cg => cg.Chat)
                .WithOne()
                .HasForeignKey<Chat>(c => c.Id);

            modelBuilder.Entity<Chat>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Chat);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany(u => u.Messages);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Chat)
                .WithMany(c => c.Messages);
        }

        public override int SaveChanges()
        {
            AddChatForNewChatGroups();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddChatForNewChatGroups();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddChatForNewChatGroups()
        {
            var newChatGroups = ChangeTracker.Entries<ChatGroup>()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity)
                .ToList();

            foreach (var chatGroup in newChatGroups)
            {
                var chat = new Chat { Id = chatGroup.Id };
                Chats.Add(chat);
                chatGroup.Chat = chat;
            }
        }
    }
}