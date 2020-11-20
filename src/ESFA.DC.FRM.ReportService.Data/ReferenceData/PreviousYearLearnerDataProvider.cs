using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class PreviousYearLearnerDataProvider : IDataProvider<IPreviousYearLearner>
    {
        public async Task<IReadOnlyCollection<IPreviousYearLearner>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
