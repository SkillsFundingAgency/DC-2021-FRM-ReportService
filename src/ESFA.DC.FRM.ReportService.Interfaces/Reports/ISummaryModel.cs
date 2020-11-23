using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.FRM.ReportService.Interfaces.Reports
{
    public interface ISummaryModel
    {
        IDictionary<string, string> HeaderData { get; }

        IList<ISummaryRow> SummaryRows { get; set; }

        int TotalRowCount { get; }
    }
}
