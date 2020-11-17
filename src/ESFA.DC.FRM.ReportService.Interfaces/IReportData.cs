using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IReportData
    {
        IReadOnlyCollection<ILearner> Learners { get; }

        IReadOnlyCollection<IOrganisation> Organisations { get; }

        IReadOnlyCollection<ILARSLearningDelivery> LARSLearningDeliveries { get; }

        IReadOnlyCollection<IMcaGlaSofLookup> McaGlaSofLookups { get; }

        IReadOnlyCollection<IMcaDevolvedContract> McaDevolvedContracts { get; }

        IReadOnlyCollection<IReturnPeriod> ReturnPeriods { get; }
    }
}
