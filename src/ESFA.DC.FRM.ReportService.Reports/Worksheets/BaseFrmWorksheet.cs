using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aspose.Cells;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets
{
    public abstract class BaseFrmWorksheet<TModel>
    {
        private readonly IExcelFileService _excelService;
        private readonly IModelBuilder<IEnumerable<TModel>> _fundingRuleMonitoringReportModelBuilder;
        private readonly IRenderService<IEnumerable<TModel>> _fundingRuleMonitoringRenderService;

        private readonly string _title;
        private readonly string _tabName;

        public string TaskName { get; }

        protected BaseFrmWorksheet(
            IExcelFileService excelService,
            IModelBuilder<IEnumerable<TModel>> fundingRulesMonitoringModelBuilder,
            IRenderService<IEnumerable<TModel>> fundingRulesMonitoringRenderService,
            string taskName,
            string tabName,
            string title)
        {
            _excelService = excelService;
            _fundingRuleMonitoringRenderService = fundingRulesMonitoringRenderService;
            _fundingRuleMonitoringReportModelBuilder = fundingRulesMonitoringModelBuilder;
            _title = title;
            _tabName = tabName;

            TaskName = taskName;
        }

        public virtual ISummaryRow Generate(Workbook workbook, IReportServiceContext reportServiceContext, IReportData reportData, CancellationToken cancellationToken)
        {
            var fundingReportMonitoringModels = _fundingRuleMonitoringReportModelBuilder.Build(reportServiceContext, reportData).ToList();

            if (fundingReportMonitoringModels.Any())
            {
                var worksheet = _excelService.GetWorksheetFromWorkbook(workbook, _tabName);

                _fundingRuleMonitoringRenderService.Render(fundingReportMonitoringModels, worksheet);
            }

            return new SummaryRow
            {
                Report = _tabName,
                Title = _title,
                NumberOfQueries = fundingReportMonitoringModels.Count
            };
        }
    }
}
