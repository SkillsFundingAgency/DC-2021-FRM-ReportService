using Aspose.Cells;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IRenderService<in T>
    {
        Worksheet Render(T model, Worksheet worksheet);
    }
}
