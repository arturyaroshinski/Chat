namespace ChatApp.Services
{
    public class EmailConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool Ssl { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Disconnect { get; set; }
    }
}
