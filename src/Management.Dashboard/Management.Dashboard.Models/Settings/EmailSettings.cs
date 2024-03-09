namespace Management.Dashboard.Models.Settings
{
    public class EmailSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? FromAddress { get; set; }

        public string? Passkey { get; set; }
    }
}
