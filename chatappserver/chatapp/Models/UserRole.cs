﻿namespace chatapp.Models
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public int GroupId { get; set; }
        public Group? Group { get; set; }

        public string? Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
