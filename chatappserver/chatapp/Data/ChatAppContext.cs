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
        public DbSet<Friends> Friends { get; set; }
        public DbSet<FriendRequests> FriendRequests { get; set; }
        public DbSet<Danhmuc> danhmuc { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMember>().HasKey(gm => new { gm.GroupId, gm.UserId });
            modelBuilder.Entity<UserRole>().HasIndex(ur => new { ur.UserId, ur.GroupId }).IsUnique();
            modelBuilder.Entity<Friends>().HasKey(gm => new {gm.UserId_1, gm.UserId_2});
            modelBuilder.Entity<Friends>()
               .HasOne(gm => gm.User_1)
               .WithMany()
               .HasForeignKey(fr => fr.UserId_1)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friends>()
                .HasOne(gm => gm.User_2)
                .WithMany()
                .HasForeignKey(fr => fr.UserId_2)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FriendRequests>().HasKey(gm => new { gm.SenderId, gm.ReceiverId });
            modelBuilder.Entity<FriendRequests>()
               .HasOne(gm => gm.Sender)
               .WithMany()
               .HasForeignKey(fr => fr.SenderId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FriendRequests>()
                .HasOne(gm => gm.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
