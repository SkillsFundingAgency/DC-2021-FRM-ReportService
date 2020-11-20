using System.Collections.Generic;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM15
{
    public class Frm15Worksheet : BaseFrmWorksheet<Frm15ReportModel>, IWorksheetReport
    {
        public Frm15Worksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<Frm15ReportModel>> fundingMonitoring06ModelBuilder,
            IRenderService<IEnumerable<Frm15ReportModel>> fundingReportMonitoringRenderService)
            : base(
                excelService,
                fundingMonitoring06ModelBuilder,
                fundingReportMonitoringRenderService,
                "TaskGenerateFundingRulesMonitoring15Report",
                "FRM15",
                "End Point Assessment Organisations")
        {
        }
    }
}
