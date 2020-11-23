using System;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class ReturnPeriod : IReturnPeriod
    {
        public string Name { get; set; }

        public int Period { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
