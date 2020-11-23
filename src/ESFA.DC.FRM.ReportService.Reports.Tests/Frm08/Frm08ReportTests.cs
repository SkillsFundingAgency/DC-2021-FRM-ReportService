using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aspose.Cells;
using ESFA.DC.ExcelService.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM08;
using ESFA.DC.ILR.Model;
using ESFA.DC.ILR.Model.Interface;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm08
{
    public class Frm08ReportTests
    {
        [Fact]
        public void GenerateAsync()
        {
            var sheetName = "FRM08";

            var cancellationToken = CancellationToken.None;

            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets.Add(sheetName);

            var frm08ReportModelBuilderMock = new Mock<IModelBuilder<IEnumerable<Frm08ReportModel>>>();
            var reportServiceContextMock = new Mock<IReportServiceContext>();
            var frm08ReportRenderServiceMock = new Mock<IRenderService<IEnumerable<Frm08ReportModel>>>();

            var reportServiceDependentData = Mock.Of<IReportData>();
            var reportModels = Enumerable.Range(1, 1).Select(x => new Frm08ReportModel()).ToList();

            frm08ReportModelBuilderMock.Setup(b => b.Build(reportServiceContextMock.Object, reportServiceDependentData)).Returns(reportModels);

            var excelServiceMock = new Mock<IExcelFileService>();

            excelServiceMock.Setup(s => s.NewWorkbook()).Returns(workbook);
            excelServiceMock.Setup(s => s.GetWorksheetFromWorkbook(workbook, sheetName)).Returns(worksheet);

            var report = NewReport(excelServiceMock.Object, frm08ReportModelBuilderMock.Object, frm08ReportRenderServiceMock.Object);

            report.Generate(workbook, reportServiceContextMock.Object, reportServiceDependentData, cancellationToken);

            frm08ReportRenderServiceMock.Verify(s => s.Render(reportModels, worksheet));
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
                                CompStatus = 6,
                                AimSeqNumber = 2,
                                LearnAimRef = "aimref",
                                FundModel = 30,
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
            var sit = new Frm08ModelBuilder();

            // Act
            var result = sit.Build(reportServiceContext.Object, reportData);

            // Assert
            result.Should().NotBeNull();
        }

        private Frm08Worksheet NewReport(
            IExcelFileService excelService = null,
            IModelBuilder<IEnumerable<Frm08ReportModel>> frm08ReportModelBuilder = null,
            IRenderService<IEnumerable<Frm08ReportModel>> frm08ReportRenderService = null)
        {
            return new Frm08Worksheet(excelService, frm08ReportModelBuilder, frm08ReportRenderService);
        }
    }
}
