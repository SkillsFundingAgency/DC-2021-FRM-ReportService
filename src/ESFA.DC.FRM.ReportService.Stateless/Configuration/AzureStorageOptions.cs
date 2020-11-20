using ESFA.DC.FRM.ReportService.Interfaces.Configuration;

namespace ESFA.DC.FRM.ReportService.Stateless.Configuration
{
    public class AzureStorageOptions : IAzureStorageOptions
    {
        public string AzureBlobConnectionString { get; set; }
    }
}
