namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IModelBuilder<out T>
    {
        T Build(IReportServiceContext reportServiceContext, IReportData reportData);
    }
}
