namespace ESFA.DC.FRM.ReportService.Interfaces.Configuration
{
    public interface IAzureStorageOptions
    {
        string AzureBlobConnectionString { get; }

        string AzureBlobContainerName { get; }
    }
}
