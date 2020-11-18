namespace ESFA.DC.FRM.ReportService.Interfaces.Configuration
{
    public interface IReportServiceConfiguration
    {
        string FCSConnectionString { get; }

        string ILR1920DataStoreConnectionString { get; }

        string ILR2021DataStoreConnectionString { get; }

        string PostcodesConnectionString { get; }

        string LarsConnectionString { get; }

        string OrgConnectionString { get; }

        string JobQueueManangerConnectionString { get; }
    }
}
