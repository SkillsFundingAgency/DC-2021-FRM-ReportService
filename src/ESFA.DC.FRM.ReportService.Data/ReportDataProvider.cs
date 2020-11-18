using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Data
{
    public class ReportDataProvider : IReportDataProvider
    {
        private readonly IDataProvider<IOrganisation> _organisationProvider;
        private readonly IDataProvider<IMcaGlaSofLookup> _mcaGlaLookupProvider;
        private readonly IDataProvider<IMcaDevolvedContract> _mcaDevolvedContractProvider;
        private readonly IDataProvider<ILARSLearningDelivery> _larsLearningDeliveryProvider;
        private readonly IDataProvider<IReturnPeriod> _returnPeriodProvider;
        private readonly IDataProvider<Learner> _learnerProvider;

        public ReportDataProvider(IDataProvider<IOrganisation> organisationProvider, IDataProvider<IMcaGlaSofLookup> mcaGlaLookupProvider, IDataProvider<IMcaDevolvedContract> mcaDevolvedContractProvider, IDataProvider<ILARSLearningDelivery> larsLearningDeliveryProvider, IDataProvider<IReturnPeriod> returnPeriodProvider, IDataProvider<Learner> learnerProvider)
        {
            _organisationProvider = organisationProvider;
            _mcaGlaLookupProvider = mcaGlaLookupProvider;
            _mcaDevolvedContractProvider = mcaDevolvedContractProvider;
            _larsLearningDeliveryProvider = larsLearningDeliveryProvider;
            _returnPeriodProvider = returnPeriodProvider;
            _learnerProvider = learnerProvider;
        }

        public async Task<IReportData> ProvideAsync(IReportServiceContext context, CancellationToken cancellationToken)
        {
            var organisationTask = _organisationProvider.ProvideAsync(context, cancellationToken);
            var mcaGlaLookupTask = _mcaGlaLookupProvider.ProvideAsync(context, cancellationToken);
            var mcaDevolvedContractTask = _mcaDevolvedContractProvider.ProvideAsync(context, cancellationToken);
            var larsLearningDeliveryTask = _larsLearningDeliveryProvider.ProvideAsync(context, cancellationToken);
            var returnPeriodTask = _returnPeriodProvider.ProvideAsync(context, cancellationToken);
            var learnerTask = _learnerProvider.ProvideAsync(context, cancellationToken);

            var tasks = new List<Task>
            {
                organisationTask,
                mcaGlaLookupTask,
                mcaDevolvedContractTask,
                larsLearningDeliveryTask,
                returnPeriodTask,
                learnerTask
            };

            await Task.WhenAll(tasks);

            return new ReportData
            {
                Organisations = organisationTask.Result,
                McaGlaSofLookups = mcaGlaLookupTask.Result,
                McaDevolvedContracts = mcaDevolvedContractTask.Result,
                LARSLearningDeliveries = larsLearningDeliveryTask.Result,
                ReturnPeriods = returnPeriodTask.Result,
                Learners = learnerTask.Result
            };
        }
    }
}
