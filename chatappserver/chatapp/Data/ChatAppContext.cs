using chatapp.Models;
using Microsoft.EntityFrameworkCore;

namespace chatapp.Data
{
    public class ChatAppContext : DbContext
    {
        public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.GroupId, gm.UserId });
            modelBuilder.Entity<UserRole>().HasIndex(ur => new { ur.UserId, ur.GroupId }).IsUnique();
        }
    }
}
