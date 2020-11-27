using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear
{
    public class ProviderSpecLearnerMonitoring : IProviderSpecLearnerMonitoring
    {
        public string LearnRefNumber { get; set; }

        public string ProvSpecLearnMonOccur { get; set; }

        public string ProvSpecLearnMon { get; set; }
    }
}
