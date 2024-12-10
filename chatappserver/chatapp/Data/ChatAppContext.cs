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
            modelBuilder.Entity<FriendRequests>().HasKey(gm => new { gm.SenderId, gm.ReceiverId });
            
            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.Group)
                .WithMany(g => g.GroupMembers)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GroupMember>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMembers)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Danhmuc: Xóa Group thì xóa luôn Danhmuc
            modelBuilder.Entity<Danhmuc>()
                .HasOne(dm => dm.Group)
                .WithMany(g => g.Danhmucs)
                .HasForeignKey(dm => dm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Channel>()
                .HasOne(dm => dm.Group)
                .WithMany(g => g.Channels)
                .HasForeignKey(dm => dm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

            // Channel: Xóa Danhmuc thì xóa luôn Channels liên quan
            modelBuilder.Entity<Channel>()
                .HasOne(c => c.Danhmuc)
                .WithMany(dm => dm.Channels)
                .HasForeignKey(c => c.DanhmucId)
                .OnDelete(DeleteBehavior.NoAction);

            // Message: Xóa Channel thì xóa luôn Messages
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Channel)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ChannelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Attachment: Xóa Message thì xóa luôn Attachment
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // Friends: Xóa User sẽ xóa các quan hệ trong Friends
            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User_1)
                .WithMany()
                .HasForeignKey(f => f.UserId_1)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Friends>()
                .HasOne(f => f.User_2)
                .WithMany()
                .HasForeignKey(f => f.UserId_2)
                .OnDelete(DeleteBehavior.NoAction);

            // FriendRequests: Xóa User sẽ xóa các quan hệ trong FriendRequests
            modelBuilder.Entity<FriendRequests>()
                .HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<FriendRequests>()
                .HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            // UserRole: Xóa User hoặc Group sẽ xóa luôn UserRole
            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Group)
                .WithMany()
                .HasForeignKey(ur => ur.GroupId)
                .OnDelete(DeleteBehavior.Cascade);

        
        }
    }
}
