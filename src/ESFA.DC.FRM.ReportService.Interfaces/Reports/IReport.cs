using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.FRM.ReportService.Interfaces.Reports
{
    public interface IReport
    {
        string TaskName { get; }

        Task<string> GenerateAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
