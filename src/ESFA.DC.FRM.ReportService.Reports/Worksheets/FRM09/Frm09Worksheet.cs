using System;
using System.Collections.Generic;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM09
{
    public class Frm09Worksheet : BaseFrmWorksheet<Frm09ReportModel>, IWorksheetReport
    {
        public Frm09Worksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<Frm09ReportModel>> fundingRulesMonitoringModelBuilder,
            IRenderService<IEnumerable<Frm09ReportModel>> fundingRulesMonitoringRenderService
        ) : base(excelService,
            fundingRulesMonitoringModelBuilder,
            fundingRulesMonitoringRenderService,
            "TaskGenerateFundingRulesMonitoring09Report",
            "FRM09",
            "Transfers with no return")
        {
        }

        public virtual IEnumerable<Type> DependsOn { get; }
    }
}
