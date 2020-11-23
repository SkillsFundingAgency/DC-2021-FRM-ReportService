using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class Organisation : IOrganisation
    {
        public long Ukprn { get; set; }

        public string Name { get; set; }
    }
}
