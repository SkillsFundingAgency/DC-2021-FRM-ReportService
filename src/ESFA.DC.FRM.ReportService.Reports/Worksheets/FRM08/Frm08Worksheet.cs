using System;
using System.Collections.Generic;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM08
{
    public class Frm08Worksheet : BaseFrmWorksheet<Frm08ReportModel>, IWorksheetReport
    {
        public Frm08Worksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<Frm08ReportModel>> fundingRulesMonitoringModelBuilder,
            IRenderService<IEnumerable<Frm08ReportModel>> fundingRulesMonitoringRenderService)
            : base(
                excelService,
                fundingRulesMonitoringModelBuilder,
                fundingRulesMonitoringRenderService,
                "TaskGenerateFundingRulesMonitoring08Report",
                "FRM08",
                "Breaks In Learning: Duration")
        {
        }

        public virtual IEnumerable<Type> DependsOn { get; }
    }
}
