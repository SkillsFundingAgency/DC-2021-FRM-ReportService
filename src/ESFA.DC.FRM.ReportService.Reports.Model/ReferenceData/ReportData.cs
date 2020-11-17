using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData
{
    public class ReportData : IReportData
    {
        public IReadOnlyCollection<ILearner> Learners { get; set; }

        public IReadOnlyCollection<IOrganisation> Organisations { get; set; }

        public IReadOnlyCollection<ILARSLearningDelivery> LARSLearningDeliveries { get; set; }

        public IReadOnlyCollection<IMcaGlaSofLookup> McaGlaSofLookups { get; set; }

        public IReadOnlyCollection<IMcaDevolvedContract> McaDevolvedContracts { get; set; }

        public IReadOnlyCollection<IReturnPeriod> ReturnPeriods { get; set; }
    }
}
