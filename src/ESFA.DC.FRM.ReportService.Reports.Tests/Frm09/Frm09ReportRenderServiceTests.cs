using System.IO;
using System.Linq;
using Aspose.Cells;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM09;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm09
{
    public class Frm09ReportRenderServiceTests
    {
        [Fact]
        public void Render()
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            var frm09ReportModels = Enumerable.Range(1, 5).Select(l => new Frm09ReportModel
            {
                LearnAimRef = l.ToString(),
                UKPRN = 1000000 + l,
                OrgName = $"Org{l}"
            });

            NewService().Render(frm09ReportModels, worksheet);

            Directory.CreateDirectory("Output");
            workbook.Save("Output/FRM09_FundingRuleMonitoringReport.xlsx");
        }

        private Frm09RenderService NewService()
        {
            return new Frm09RenderService();
        }
    }
}
