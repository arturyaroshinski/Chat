using System.Collections.Generic;

namespace Chat.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<User> Users { get; set; }
    }
}