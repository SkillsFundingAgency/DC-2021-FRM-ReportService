﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM08;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm08
{
    public class Frm08ReportRenderServiceTests
    {
        [Fact]
        public void Render()
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];

            var frm08ReportModels = Enumerable.Range(1, 5).Select(l => new Frm08ReportModel
            {
                LearnAimRef = l.ToString(),
                UKPRN = 1000000 + l,
                OrgName = $"Org{l}"
            });

            NewService().Render(frm08ReportModels, worksheet);

            Directory.CreateDirectory("Output");
            workbook.Save("Output/FRM08_FundingRuleMonitoringReport.xlsx");
        }

        private Frm08RenderService NewService()
        {
            return new Frm08RenderService();
        }
    }
}
