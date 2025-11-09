namespace Infrastructure.Configurations
{
    using Application.Interfaces;

    public class FrontendSettings : IFrontendSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
