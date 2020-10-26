using System;

namespace ChatApp.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }
    }
}