using ESFA.DC.FRM.ReportService.Interfaces.Configuration;

namespace ESFA.DC.FRM.ReportService.Stateless.Configuration
{
    public class ReportServiceConfiguration : IReportServiceConfiguration
    {
        public string FCSConnectionString { get; set; }

        public string ILR1920DataStoreConnectionString { get; set; }

        public string ILR2021DataStoreConnectionString { get; set; }

        public string LarsConnectionString { get; set; }

        public string OrgConnectionString { get; set; }

        public string JobQueueManangerConnectionString { get; set; }
    }
}
