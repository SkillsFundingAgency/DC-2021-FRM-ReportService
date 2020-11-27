using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;
using ESFA.DC.FRM.ReportService.Reports.Constants;
using ESFA.DC.FRM.ReportService.Reports.Model.Summary;
using ESFA.DC.FRM.ReportService.Reports.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Summary
{
    public class FrmSummaryModelBuilder : BaseFrmModelBuilder, IModelBuilder<ISummaryModel>
    {
        public ISummaryModel Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var organisationNameDictionary = reportData.Organisations.ToDictionary(x => x.Ukprn, x => x.Name);
            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            return new SummaryModel
            {
                HeaderData = BuildHeader(
                    orgName,
                    reportServiceContext.Ukprn.ToString(),
                    string.Empty,
                    string.Empty,
                    ReportingConstants.OfficialSensitive)
            };
        }

        public IDictionary<string, string> BuildHeader(string providerName, string UKPRN, string ilrFileName, string lastIlrFileUpdate, string securityClassification)
        {
            return new Dictionary<string, string>
            {
                { "Provider Name:", providerName },
                { "UKPRN:", UKPRN },
                { "ILR File:",  ilrFileName },
                { "Last ILR File Update:", lastIlrFileUpdate },
                { "Security Classification", securityClassification }
            };
        }
    }
}
