using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class LARSLearningDelivery : ILARSLearningDelivery
    {
        public string LearnAimRef { get; set; }

        public string LearnAimRefTitle { get; set; }

        public string LearnAimRefTypeDesc { get; set; }

        public IReadOnlyCollection<ILARSLearningDeliveryCategory> LARSLearningDeliveryCategories { get; set; }
    }
}
