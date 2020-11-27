using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Cells;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;
using ESFA.DC.FRM.ReportService.Reports.Model.Summary;
using Moq;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Summary
{
    public class FrmSummaryReportTests
    {
        [Fact]
        public async Task GenerateAsync()
        {
            //Data
            var sheetName = "Summary";
            var fileName = "FileName";
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add(sheetName);
            var dictionary = new Dictionary<string, IWorksheetReport>();

            //Mocks
            var fundingSummaryReportModelBuilderMock = new Mock<IModelBuilder<ISummaryModel>>();
            var reportServiceContextMock = new Mock<IReportServiceContext>();
            var reportDataMock = new Mock<IReportData>();
            var excelServiceMock = new Mock<IExcelFileService>();
            var fileNameServiceMock = new Mock<IFileNameService>();
            var fundingSummaryReportRenderServiceMock = new Mock<IRenderService<ISummaryModel>>();
            var reportDataProviderMock = new Mock<IReportDataProvider>();
            var cancellationToken = CancellationToken.None;

            var model = new SummaryModel();

            //Setup
            reportDataProviderMock.Setup(x => x.ProvideAsync(reportServiceContextMock.Object, cancellationToken))
                .ReturnsAsync(reportDataMock.Object);

            fundingSummaryReportModelBuilderMock.Setup(b => b.Build(reportServiceContextMock.Object, reportDataMock.Object)).Returns(model);
            fileNameServiceMock.Setup(s => s.GetFilename(reportServiceContextMock.Object, "Frm Summary Report", OutputTypes.Excel, true)).Returns(fileName);
            excelServiceMock.Setup(s => s.NewWorkbook()).Returns(workbook);
            excelServiceMock.Setup(s => s.GetWorksheetFromWorkbook(workbook, sheetName)).Returns(worksheet);

            await NewReport(dictionary, fileNameServiceMock.Object, excelServiceMock.Object, reportDataProviderMock.Object, fundingSummaryReportModelBuilderMock.Object, fundingSummaryReportRenderServiceMock.Object).GenerateAsync(reportServiceContextMock.Object, cancellationToken);

            fundingSummaryReportModelBuilderMock.Verify(x => x.Build(reportServiceContextMock.Object, reportDataMock.Object));
            fundingSummaryReportRenderServiceMock.Verify(s => s.Render(model, worksheet));
        }

        private Frm NewReport(
            IDictionary<string, IWorksheetReport> frmReports = null,
            IFileNameService fileNameService = null,
            IExcelFileService excelFileService = null,
            IReportDataProvider reportDataProvider = null,
            IModelBuilder<ISummaryModel> modelBuilder = null,
            IRenderService<ISummaryModel> renderService = null)
        {
            return new Frm(frmReports, excelFileService, fileNameService, reportDataProvider, modelBuilder, renderService);
        }
    }
}
