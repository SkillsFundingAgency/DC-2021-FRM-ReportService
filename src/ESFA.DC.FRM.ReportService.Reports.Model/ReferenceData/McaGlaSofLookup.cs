using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class McaGlaSofLookup : IMcaGlaSofLookup
    {
        public string SofCode { get; set; }

        public string McaGlaShortCode { get; set; }

        public string McaGlaFullName { get; set; }
    }
}
