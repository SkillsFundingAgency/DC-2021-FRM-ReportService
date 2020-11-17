using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;
using ESFA.DC.FRM.ReportService.Reports.Extensions;

namespace ESFA.DC.FRM.ReportService.Reports
{
    public class Frm : IReport
    {
        private readonly IFileNameService _fileNameService;
        private readonly IExcelFileService _excelFileService;
        private readonly IDictionary<string, IWorksheetReport> _worksheets;
        private readonly IReportDataProvider _reportDataProvider;

        private string SummaryName => "Summary";

        public Frm(IDictionary<string, IWorksheetReport> worksheets, IExcelFileService excelFileService, IFileNameService fileNameService, IReportDataProvider reportDataProvider)
        {
            _excelFileService = excelFileService;
            _fileNameService = fileNameService;
            _worksheets = worksheets;
            _reportDataProvider = reportDataProvider;
        }

        public string TaskName => "TaskGenerateFundingRulesMonitoringReport";

        public async Task<string> GenerateAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var fileName = _fileNameService.GetFilename(reportServiceContext, "", OutputTypes.Excel);

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

//                var frmSummaryReport = _frmSummaryReportModelBuilder.Build(reportServiceContext, reportsDependentData);
//                frmSummaryReport.SummaryTable = summaries;
//                _frmSummaryReportRenderService.Render(frmSummaryReport, summaryWorksheet);

                await _excelFileService.SaveWorkbookAsync(workbook, fileName, reportServiceContext.Container, cancellationToken);
            }

            return fileName;
        }
    }
}
