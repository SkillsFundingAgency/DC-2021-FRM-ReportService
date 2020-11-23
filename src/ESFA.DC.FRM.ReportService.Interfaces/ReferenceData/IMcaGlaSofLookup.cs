namespace ESFA.DC.FRM.ReportService.Interfaces.ReferenceData
{
    public interface IMcaGlaSofLookup
    {
        string SofCode { get; }

        string McaGlaShortCode { get; }

        string McaGlaFullName { get; }
    }
}
