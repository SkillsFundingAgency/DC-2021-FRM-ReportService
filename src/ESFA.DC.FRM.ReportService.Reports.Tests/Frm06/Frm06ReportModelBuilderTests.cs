using System;
using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;
using ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06;
using FluentAssertions;
using Moq;
using Xunit;

namespace ESFA.DC.FRM.ReportService.Reports.Tests.Frm06
{
    public class Frm06ReportModelBuilderTests
    {
        [Fact]
        public void Build()
        {
            var organistions = new List<Organisation>
            {
                new Organisation
                {
                    Ukprn = 1,
                    Name = "OrgName1",
                },
                new Organisation
                {
                    Ukprn = 2,
                    Name = "OrgName2",
                }
            };

            var larsLearningDeliveries = new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "LearnAimRef1",
                    LearnAimRefTypeDesc = "LeanAimRef1Desc"
                }
            };

            var previousYearLearners = new List<IPreviousYearLearner>
            {
                new PreviousYearLearner
                {
                    LearnRefNumber = "LearnRefNumber1",
                    LearnAimRef = "LearnAimRef1",
                    FundModel = 35,
                },
                new PreviousYearLearner
                {
                    LearnRefNumber = "LearnRefNumber2",
                    LearnAimRef = "LearnAimRef1",
                    FundModel = 99,
                    LearningDeliveryFAMs = new List<LearningDeliveryFAM>
                    {
                        new LearningDeliveryFAM
                        {
                            LearnDelFAMCode = "1",
                            LearnDelFAMType = "ADL"
                        }
                    }
                },
                new PreviousYearLearner
                {
                    LearnRefNumber = "LearnRefNumber3",
                    LearnAimRef = "LearnAimRef1",
                    FundModel = 99,
                },
            };

            var context = new Mock<IReportServiceContext>();
            context.Setup(x => x.ReturnPeriodName).Returns("R01");

            var refData = new Mock<IReportData>();

            refData.Setup(x => x.PreviousYearLearners).Returns(previousYearLearners);
            refData.Setup(x => x.LARSLearningDeliveries).Returns(larsLearningDeliveries);
            refData.Setup(x => x.Organisations).Returns(organistions);

            var expectedModels = new List<Frm06ReportModel>
            {
                new Frm06ReportModel
                {
                    LearnAimRef = "LearnAimRef1",
                    FundingModel = 35,
                    AdvancedLoansIndicator = string.Empty,
                    ResIndicator = string.Empty,
                    Return = "R01",
                    LearnRefNumber = "LearnRefNumber1",
                    SOFCode = string.Empty,
                    LearningAimType = "LeanAimRef1Desc"
                },
                new Frm06ReportModel
                {
                    LearnAimRef = "LearnAimRef1",
                    FundingModel = 99,
                    AdvancedLoansIndicator = "1",
                    ResIndicator = string.Empty,
                    Return = "R01",
                    LearnRefNumber = "LearnRefNumber2",
                    SOFCode = string.Empty,
                    LearningAimType = "LeanAimRef1Desc"
                }
            };

            NewBuilder().Build(context.Object, refData.Object).Should().BeEquivalentTo(expectedModels);
        }

        [Fact]
        public void Build_NoData()
        {
            var organisations = new List<Organisation>
            {
                new Organisation
                {
                    Ukprn = 1,
                    Name = "OrgName1",
                },
                new Organisation
                {
                    Ukprn = 2,
                    Name = "OrgName2",
                }
            };

            var larsLearningDeliveries = new List<LARSLearningDelivery>
            {
                new LARSLearningDelivery
                {
                    LearnAimRef = "LearnAImRef1",
                    LearnAimRefTypeDesc = "LeanAimRef1Desc"
                }
            };

            var context = new Mock<IReportServiceContext>();
            context.Setup(x => x.ReturnPeriodName).Returns("R01");

            var refData = new Mock<IReportData>();
            refData.Setup(x => x.Organisations).Returns(organisations);
            refData.Setup(x => x.LARSLearningDeliveries).Returns(larsLearningDeliveries);
            refData.Setup(x => x.PreviousYearLearners).Returns(Array.Empty<IPreviousYearLearner>());

            NewBuilder().Build(context.Object, refData.Object).Should().BeNullOrEmpty();
        }

        private Frm06ModelBuilder NewBuilder()
        {
            return new Frm06ModelBuilder(new LearnerComparer());
        }
    }
}
