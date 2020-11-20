using System;
using System.Collections.Generic;

namespace ESFA.DC.FRM.ReportService.Interfaces.PreviousYear
{
    public interface IPreviousYearLearner
    {
        long UKPRN { get; }

        string OrgName { get; }

        long ULN { get; }

        int AimSeqNumber { get; }

        string LearnRefNumber { get; }

        string LearnAimRef { get; }

        string LearnAimTitle { get; }

        int? ProgTypeNullable { get; }

        int? StdCodeNullable { get; }

        int? FworkCodeNullable { get; }

        int? PwayCodeNullable { get; }

        DateTime LearnStartDate { get; }

        int AimType { get; }

        int FundModel { get; }

        long? PrevUKPRN { get; }

        long? PMUKPRN { get; }

        long? PartnerUKPRN { get; }

        string PartnerOrgName { get; }

        string PrevLearnRefNumber { get; }

        string SWSupAimId { get; }

        DateTime LearnPlanEndDate { get; }

        DateTime? LearnActEndDate { get; }

        DateTime? OrigLearnStartDate { get; }

        int? PriorLearnFundAdj { get; }

        int? OtherFundAdj { get; }

        int CompStatus { get; }

        int? Outcome { get; }

        IReadOnlyCollection<ILearningDeliveryFAM> LearningDeliveryFAMs { get; }

        IReadOnlyCollection<IProviderSpecLearnerMonitoring> ProviderSpecLearnerMonitorings { get; }

        IReadOnlyCollection<IProviderSpecDeliveryMonitoring> ProvSpecDeliveryMonitorings { get; }
    }
}
