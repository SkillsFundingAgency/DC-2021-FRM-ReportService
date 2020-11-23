using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;

namespace ESFA.DC.FRM.ReportService.Reports
{
    public class Frm : IReport
    {
        private readonly IFileNameService _fileNameService;
        private readonly IExcelFileService _excelFileService;
        private readonly IDictionary<string, IWorksheetReport> _worksheets;
        private readonly IReportDataProvider _reportDataProvider;
        private readonly IModelBuilder<ISummaryModel> _summaryPageModelBuilder;
        private readonly IRenderService<ISummaryModel> _summarPageRenderService;

        private readonly string SummaryName = "Summary";

        private readonly string FileName = "Funding Rules Monitoring Report";

        public Frm(IDictionary<string, IWorksheetReport> worksheets, IExcelFileService excelFileService, IFileNameService fileNameService, IReportDataProvider reportDataProvider, IModelBuilder<ISummaryModel> summaryPageModelBuilder, IRenderService<ISummaryModel> summaryPageRenderService)
        {
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _worksheets = worksheets;
            _reportDataProvider = reportDataProvider;
            _summaryPageModelBuilder = summaryPageModelBuilder;
            _summarPageRenderService = summaryPageRenderService;
        }

        public string TaskName => "TaskGenerateFundingRulesMonitoringReport";

        public async Task<string> GenerateAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var fileName = _fileNameService.GetFilename(reportServiceContext, FileName, OutputTypes.Excel);

            var reportData = await _reportDataProvider.ProvideAsync(reportServiceContext, cancellationToken);

            using (var workbook = _excelFileService.NewWorkbook())
            {
                workbook.Worksheets.Clear();
                var summaries = new List<ISummaryRow>();
                var summaryWorksheet = _excelFileService.GetWorksheetFromWorkbook(workbook, SummaryName);

                foreach (var worksheetTask in reportServiceContext.Tasks)
                {
                    var worksheet = _worksheets.GetValueOrDefault(worksheetTask);

                    summaries.Add(worksheet.Generate(workbook, reportServiceContext, reportData, cancellationToken));
                }

                var summaryModel = _summaryPageModelBuilder.Build(reportServiceContext, reportData);
                summaryModel.SummaryRows = summaries;

                _summarPageRenderService.Render(summaryModel, summaryWorksheet);

                await _excelFileService.SaveWorkbookAsync(workbook, fileName, reportServiceContext.Container, cancellationToken);
            }

            return fileName;
        }
    }
}
