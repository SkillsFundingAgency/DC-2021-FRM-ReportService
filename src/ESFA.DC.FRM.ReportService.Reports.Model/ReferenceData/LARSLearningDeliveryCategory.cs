using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class LARSLearningDeliveryCategory : ILARSLearningDeliveryCategory
    {
        public string LearnAimRef { get; set; }

        public int CategoryRef { get; set; }
    }
}
