using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IReportData
    {
        IReadOnlyCollection<Learner> Learners { get; }

        IReadOnlyCollection<IOrganisation> Organisations { get; }

        IReadOnlyCollection<ILARSLearningDelivery> LARSLearningDeliveries { get; }

        IReadOnlyCollection<IMcaGlaSofLookup> McaGlaSofLookups { get; }

        IReadOnlyCollection<IMcaDevolvedContract> McaDevolvedContracts { get; }

        IReadOnlyCollection<IReturnPeriod> ReturnPeriods { get; }

        IReadOnlyCollection<IPreviousYearLearner> PreviousYearLearners { get; }
    }
}
