using Chat.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chat.Database
{
    public class ChatDbContext : IdentityDbContext<User>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

        public DbSet<Chat.Models.Chat> Chats { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}