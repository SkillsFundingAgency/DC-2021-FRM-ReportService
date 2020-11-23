using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Aspose.Cells;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM15;
using ESFA.DC.ILR.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm15
{
    public class Frm15ReportTests
    {
        [Fact]
        public void GenerateAsync()
        {
            var sheetName = "FRM15";

            var cancellationToken = CancellationToken.None;

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add(sheetName);

            var frm15ReportModelBuilderMock = new Mock<IModelBuilder<IEnumerable<Frm15ReportModel>>>();
            var reportServiceContextMock = new Mock<IReportServiceContext>();
            var frm15ReportRenderServiceMock = new Mock<IRenderService<IEnumerable<Frm15ReportModel>>>();

            var reportData = Mock.Of<IReportData>();
            var reportModels = Enumerable.Range(1, 1).Select(x => new Frm15ReportModel()).ToList();

            frm15ReportModelBuilderMock.Setup(b => b.Build(reportServiceContextMock.Object, reportData)).Returns(reportModels);

            var excelServiceMock = new Mock<IExcelFileService>();

            excelServiceMock.Setup(s => s.NewWorkbook()).Returns(workbook);
            excelServiceMock.Setup(s => s.GetWorksheetFromWorkbook(workbook, sheetName)).Returns(worksheet);

            var report = NewReport(excelServiceMock.Object, frm15ReportModelBuilderMock.Object, frm15ReportRenderServiceMock.Object);

            report.Generate(workbook, reportServiceContextMock.Object, reportData, cancellationToken);

            frm15ReportRenderServiceMock.Verify(s => s.Render(reportModels, worksheet));
        }

        [Fact]
        public void BuildHandlesMissingLearningDeliveryProviderSpecDeliveryMonitorings()
        {
            // Arrange
            var message = new Message
            {
                Learner = new MessageLearner[]
                {
                    new MessageLearner
                    {
                        LearningDelivery = new MessageLearnerLearningDelivery[]
                        {
                            new MessageLearnerLearningDelivery
                            {
                                CompStatus = 4,
                                AimSeqNumber = 2,
                                LearnAimRef = "aimref",
                                FundModel = 36,
                                LearningDeliveryFAM = new MessageLearnerLearningDeliveryLearningDeliveryFAM[]
                                {
                                    new MessageLearnerLearningDeliveryLearningDeliveryFAM
                                    {
                                        LearnDelFAMType = "RES"
                                    }
                                },
                                LearnActEndDate = new DateTime(2018, 1, 1),
                                LearnActEndDateSpecified = true
                            },
                        }
                    }
                }
            };

            var reportData = new ReportData
            {
                Organisations = new List<Organisation> { new Organisation { Name = "org", Ukprn = 123 } },
                ReturnPeriods = new HashSet<ReturnPeriod> { new ReturnPeriod { Start = new DateTime(2020, 01, 01), End = new DateTime(2020, 01, 01) } },
                LARSLearningDeliveries = new HashSet<LARSLearningDelivery>
                {
                    new LARSLearningDelivery
                    {
                        LearnAimRef = "aimref",
                        LARSLearningDeliveryCategories = new HashSet<LARSLearningDeliveryCategory>
                        {
                            new LARSLearningDeliveryCategory
                            {
                                CategoryRef = 123
                            }
                        },
                    }
                }
            };

            var reportServiceContext = new Mock<IReportServiceContext>();
            reportServiceContext.Setup(s => s.ReturnPeriod).Returns(10);
            reportServiceContext.Setup(s => s.Ukprn).Returns(123);
            reportServiceContext.Setup(s => s.SubmissionDateTimeUtc).Returns(new DateTime(2020, 01, 01));

            var sit = new Frm15ModelBuilder();

            // Act
            var result = sit.Build(reportServiceContext.Object, reportData);

            // Assert
            result.Should().NotBeNull();
        }

        private Frm15Worksheet NewReport(
            IExcelFileService excelService = null,
            IModelBuilder<IEnumerable<Frm15ReportModel>> frm15ReportModelBuilder = null,
            IRenderService<IEnumerable<Frm15ReportModel>> frm15ReportRenderService = null)
        {
            return new Frm15Worksheet(excelService, frm15ReportModelBuilder, frm15ReportRenderService);
        }
    }
}
