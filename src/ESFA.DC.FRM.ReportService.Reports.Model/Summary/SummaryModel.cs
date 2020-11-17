using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Summary
{
    public class SummaryModel : ISummaryModel
    {
        public IDictionary<string, string> HeaderData { get; set; }
        public IList<ISummaryRow> SummaryRows { get; set; }
        public int TotalRowCount { get; set; }
    }
}
