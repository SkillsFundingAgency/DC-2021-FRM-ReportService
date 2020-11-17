using System;
using System.Collections.Generic;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM07
{
    public class Frm07Worksheet : BaseFrmWorksheet<Frm07ReportModel>, IWorksheetReport
    {
        public Frm07Worksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<Frm07ReportModel>> fundingRulesMonitoringModelBuilder,
            IRenderService<IEnumerable<Frm07ReportModel>> fundingRulesMonitoringRenderService)
            : base(excelService,
                fundingRulesMonitoringModelBuilder,
                fundingRulesMonitoringRenderService,
                "TaskGenerateFundingRulesMonitoring07Report",
                "FRM07",
                "Breaks In Learning: Planned End Date")
        {
        }

        public virtual IEnumerable<Type> DependsOn { get; }
    }
}
