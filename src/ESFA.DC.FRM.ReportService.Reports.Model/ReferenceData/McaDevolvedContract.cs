using System;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class McaDevolvedContract : IMcaDevolvedContract
    {
        public string McaGlaShortCode { get; set; }

        public int Ukprn { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
