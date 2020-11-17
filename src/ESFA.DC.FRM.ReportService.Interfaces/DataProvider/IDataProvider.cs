using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ESFA.DC.FRM.ReportService.Interfaces.DataProvider
{
    public interface IDataProvider<T>
    {
        Task<IReadOnlyCollection<T>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken);
    }
}
