using System;
using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear
{
    public class PreviousYearLearner : IPreviousYearLearner
    {
        public long UKPRN { get; set; }

        public string OrgName { get; set; }

        public long ULN { get; set; }

        public int AimSeqNumber { get; set; }

        public string LearnRefNumber { get; set; }

        public string LearnAimRef { get; set; }

        public string LearnAimTitle { get; set; }

        public int? ProgTypeNullable { get; set; }

        public int? StdCodeNullable { get; set; }

        public int? FworkCodeNullable { get; set; }

        public int? PwayCodeNullable { get; set; }

        public DateTime LearnStartDate { get; set; }

        public int AimType { get; set; }

        public int FundModel { get; set; }

        public long? PrevUKPRN { get; set; }

        public long? PMUKPRN { get; set; }

        public long? PartnerUKPRN { get; set; }

        public string PartnerOrgName { get; set; }

        public string PrevLearnRefNumber { get; set; }

        public string SWSupAimId { get; set; }

        public DateTime LearnPlanEndDate { get; set; }

        public DateTime? LearnActEndDate { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public int? PriorLearnFundAdj { get; set; }

        public int? OtherFundAdj { get; set; }

        public int CompStatus { get; set; }

        public int? Outcome { get; set; }

        public IReadOnlyCollection<ILearningDeliveryFAM> LearningDeliveryFAMs { get; set; }

        public IReadOnlyCollection<IProviderSpecLearnerMonitoring> ProviderSpecLearnerMonitorings { get; set; }

        public IReadOnlyCollection<IProviderSpecDeliveryMonitoring> ProvSpecDeliveryMonitorings { get; set; }
    }
}
