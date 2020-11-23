using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06;
using FluentAssertions;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm06
{
    public class Frm06ReportRenderServiceTests
    {
        [Fact]
        public void Render()
        {
            var expectedColCount = 36;
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            var frm06ReportModels = Enumerable.Range(1, 5).Select(l => new Frm06ReportModel
            {
                LearnAimRef = l.ToString(),
                UKPRN = 1000000 + l,
                OrgName = $"Org{l}"
            });

            var sheet = NewService().Render(frm06ReportModels, worksheet);

            // get column counts. Zero indexed so add 1.
            var headerColCount = sheet.Cells.CheckRow(sheet.Cells.MinRow).LastDataCell.Column + 1;
            var maxCol = sheet.Cells.MaxDataColumn + 1;

            Directory.CreateDirectory("Output");
            workbook.Save("Output/FRM06_FundingRuleMonitoringReport.xlsx");

            headerColCount.Should().Be(expectedColCount);
            maxCol.Should().Be(expectedColCount);
        }

        private Frm06RenderService NewService()
        {
            return new Frm06RenderService();
        }
    }
}
