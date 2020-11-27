using ESFA.DC.FRM.ReportService.Interfaces;

namespace ESFA.DC.FRM.ReportService.Reports.Model
{
    public class SummaryRow : ISummaryRow
    {
        public string Report { get; set; }

        public string Title { get; set; }

        public int NumberOfQueries { get; set; }
    }
}
