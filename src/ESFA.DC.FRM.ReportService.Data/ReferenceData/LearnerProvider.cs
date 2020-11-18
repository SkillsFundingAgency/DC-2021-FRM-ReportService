using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.ILR2021.DataStore.EF;
using ESFA.DC.ILR2021.DataStore.EF.Interface;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class LearnerProvider : IDataProvider<Learner>
    {
        private readonly Func<IIlr2021Context> _ilrContextFunc;

        public LearnerProvider(Func<IIlr2021Context> ilrContextFunc)
        {
            _ilrContextFunc = ilrContextFunc;
        }

        public async Task<IReadOnlyCollection<Learner>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var context = _ilrContextFunc())
            {
                var learners = context.Learners.Where(x => x.UKPRN == reportServiceContext.Ukprn).ToList();

                return await Task.FromResult(learners);
            }
        }
    }
}
