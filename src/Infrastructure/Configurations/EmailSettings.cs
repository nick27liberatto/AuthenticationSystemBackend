namespace Infrastructure.Configurations
{
    public class EmailSettings
    {
        public string From { get; set; } = string.Empty;
        public SmtpSettings Smtp { get; set; } = new SmtpSettings();
    }
}
