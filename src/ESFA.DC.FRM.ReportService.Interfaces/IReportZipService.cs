using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IReportZipService
    {
        Task CreateOrUpdateZipWithReportAsync(string reportFileNameKey, IReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
