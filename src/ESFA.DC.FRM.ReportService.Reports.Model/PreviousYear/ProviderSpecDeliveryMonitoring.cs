using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear
{
    public class ProviderSpecDeliveryMonitoring : IProviderSpecDeliveryMonitoring
    {
        public string LearnRefNumber { get; set; }

        public int AimSeqNumber { get; set; }

        public string ProvSpecDelMonOccur { get; set; }

        public string ProvSpecDelMon { get; set; }
    }
}
