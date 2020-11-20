using System;

namespace ESFA.DC.FRM.ReportService.Interfaces.PreviousYear
{
    public interface ILearningDeliveryFAM
    {
        string LearnDelFAMType { get; }

        string LearnDelFAMCode { get; }

        DateTime? LearnDelFAMDateFrom { get; }

        DateTime? LearnDelFAMDateTo { get; }
    }
}
