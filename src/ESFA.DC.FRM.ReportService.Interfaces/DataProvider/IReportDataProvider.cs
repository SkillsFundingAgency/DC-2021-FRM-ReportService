using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.FRM.ReportService.Interfaces.DataProvider
{
    public interface IReportDataProvider
    {
        Task<IReportData> ProvideAsync(IReportServiceContext context, CancellationToken cancellationToken);
    }
}
