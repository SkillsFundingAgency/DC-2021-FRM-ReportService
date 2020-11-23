using System.Collections.Generic;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06
{
    public class Frm06Worksheet : BaseFrmWorksheet<Frm06ReportModel>, IWorksheetReport
    {
        public Frm06Worksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<Frm06ReportModel>> fundingRulesMonitoringModelBuilder,
            IRenderService<IEnumerable<Frm06ReportModel>> fundingRulesMonitoringRenderService)
            : base(
                excelService,
                fundingRulesMonitoringModelBuilder,
                fundingRulesMonitoringRenderService,
                "TaskGenerateFundingRulesMonitoring06Report",
                "FRM06",
                "Continuance Issues")
        {
        }
    }
}
