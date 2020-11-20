using System;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear
{
    public class LearningDeliveryFAM : ILearningDeliveryFAM
    {
        public string LearnRefNumber { get; set; }

        public int AimSeqNumber { get; set; }

        public string LearnDelFAMType { get; set; }

        public string LearnDelFAMCode { get; set; }

        public DateTime? LearnDelFAMDateFrom { get; set; }

        public DateTime? LearnDelFAMDateTo { get; set; }
    }
}
