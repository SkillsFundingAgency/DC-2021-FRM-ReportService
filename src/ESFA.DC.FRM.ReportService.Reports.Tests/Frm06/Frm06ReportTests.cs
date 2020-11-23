using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aspose.Cells;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06;
using Moq;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm06
{
    public class Frm06ReportTests
    {
        [Fact]
        public void GenerateAsync()
        {
            var sheetName = "FRM06";

            var cancellationToken = CancellationToken.None;

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add(sheetName);

            var frm06ReportModelBuilderMock = new Mock<IModelBuilder<IEnumerable<Frm06ReportModel>>>();
            var reportServiceContextMock = new Mock<IReportServiceContext>();
            var frm06ReportRenderServiceMock = new Mock<IRenderService<IEnumerable<Frm06ReportModel>>>();

            var reportData = Mock.Of<IReportData>();
            var reportModels = Enumerable.Range(1, 1).Select(x => new Frm06ReportModel()).ToList();

            frm06ReportModelBuilderMock.Setup(b => b.Build(reportServiceContextMock.Object, reportData)).Returns(reportModels);

            var excelServiceMock = new Mock<IExcelFileService>();

            excelServiceMock.Setup(s => s.NewWorkbook()).Returns(workbook);
            excelServiceMock.Setup(s => s.GetWorksheetFromWorkbook(workbook, sheetName)).Returns(worksheet);

            var report = NewReport(excelServiceMock.Object, frm06ReportModelBuilderMock.Object, frm06ReportRenderServiceMock.Object);

            report.Generate(workbook, reportServiceContextMock.Object, reportData, cancellationToken);

            frm06ReportRenderServiceMock.Verify(s => s.Render(reportModels, worksheet));
        }

        private Frm06Worksheet NewReport(
            IExcelFileService excelService = null,
            IModelBuilder<IEnumerable<Frm06ReportModel>> frm06ReportModelBuilder = null,
            IRenderService<IEnumerable<Frm06ReportModel>> frm06ReportRenderService = null)
        {
            return new Frm06Worksheet(excelService, frm06ReportModelBuilder, frm06ReportRenderService);
        }
    }
}