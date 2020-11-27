using System.Threading;
using Aspose.Cells;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IWorksheetReport
    {
        string TaskName { get; }

        ISummaryRow Generate(Workbook workbook, IReportServiceContext reportServiceContext, IReportData reportData, CancellationToken cancellationToken);
    }
}
