namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IFileNameService
    {
        string GetFilename(IReportServiceContext reportServiceContext, string fileName, OutputTypes outputType, bool includeDateTime = true);
    }
}
