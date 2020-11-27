using System.IO;
using System.Linq;
using Aspose.Cells;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM15;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm15
{
    public class Frm15ReportRenderServiceTests
    {
        [Fact]
        public void Render()
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            var frm08ReportModels = Enumerable.Range(1, 5).Select(l => new Frm15ReportModel
            {
                LearnAimRef = l.ToString(),
                UKPRN = 1000000 + l,
                OrgName = $"Org{l}"
            });

            NewService().Render(frm08ReportModels, worksheet);

            Directory.CreateDirectory("Output");
            workbook.Save("Output/FRM15_FundingRuleMonitoringReport.xlsx");
        }

        private Frm15RenderService NewService()
        {
            return new Frm15RenderService();
        }
    }
}
