namespace MoneyTransfer.Service
{
    public class ServiceConfiguration
    {
        public string ConfigurationFilePath { get; set; }
            = $"{nameof(ServiceConfiguration).ToLowerInvariant()}.cfg";

        public string EnvironmentName { get; set; }
            = "Production";
    }
}
