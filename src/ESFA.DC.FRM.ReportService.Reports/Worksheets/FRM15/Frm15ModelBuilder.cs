using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM15
{
    public class Frm15ModelBuilder : BaseFrmModelBuilder, IModelBuilder<IEnumerable<Frm15ReportModel>>
    {
        private readonly int _includedCompStatus = 1;
        private readonly int _includedAimType = 1;
        private readonly int _includedFundModel = 36;
        private readonly int _includedProgType = 25;

        private readonly string AFinTypeTNP = "TNP";
        private readonly string AFinTypePMR = "PMR";
        private readonly int AFinCode = 2;

        public IEnumerable<Frm15ReportModel> Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var models = new List<Frm15ReportModel>();
            var returnPeriod = reportServiceContext.ReturnPeriodName;

            var organisationNameDictionary = reportData.Organisations.ToDictionary(x => x.Ukprn, x => x.Name);

            var learnAimDictionary = reportData.LARSLearningDeliveries.ToDictionary(x => x.LearnAimRef, x => x, StringComparer.OrdinalIgnoreCase);

            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            var deliveries = reportData.Learners
                                        ?.SelectMany(l => l.LearningDeliveries.Where(ld =>
                                            ld.FundModel == _includedFundModel
                                            && ld.ProgType == _includedProgType
                                            && ld.AimType == _includedAimType
                                            && ld.CompStatus == _includedCompStatus
                                            && ld.EPAOrgID == null).Select(ld => new { Learner = l, LearningDelivery = ld }));

            var currentReturnEndDate = reportData.ReturnPeriods.FirstOrDefault(d => reportServiceContext.SubmissionDateTimeUtc >= d.Start && reportServiceContext.SubmissionDateTimeUtc <= d.End).End;

            if (deliveries == null)
            {
                return Enumerable.Empty<Frm15ReportModel>();
            }

            foreach (var delivery in deliveries)
            {
                if ((delivery.LearningDelivery.LearnPlanEndDate > currentReturnEndDate && DaysBetween(currentReturnEndDate, delivery.LearningDelivery.LearnPlanEndDate) <= 90) || currentReturnEndDate > delivery.LearningDelivery.LearnPlanEndDate)
                {
                    var aFinAmount = delivery.LearningDelivery.AppFinRecords
                        ?.OrderByDescending(afr => afr.AFinDate).FirstOrDefault(afr => afr.AFinType == AFinTypeTNP && afr.AFinCode == AFinCode)?.AFinAmount;

                    var paymentsReceived = delivery.LearningDelivery.AppFinRecords
                        ?.Where(afr => afr.AFinType == AFinTypePMR && afr.AFinCode == AFinCode)
                        .Sum(afr => afr.AFinAmount);

                    var advancedLoansIndicator = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, ADLLearnDelFamType);
                    var resIndicator = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, RESLearnDelFamType);
                    var sofCode = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, SOFLearnDelFamType);

                    var pmOrgName = organisationNameDictionary.GetValueOrDefault(
                            delivery.Learner.PMUKPRN.GetValueOrDefault());
                    var prevOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PrevUKPRN
                            .GetValueOrDefault());
                    var partnerOrgName = organisationNameDictionary.GetValueOrDefault(delivery.LearningDelivery.PartnerUKPRN.GetValueOrDefault());
                    var learnAim = learnAimDictionary.GetValueOrDefault(delivery.LearningDelivery.LearnAimRef);

                    models.Add(new Frm15ReportModel
                    {
                        Return = returnPeriod,
                        UKPRN = reportServiceContext.Ukprn,
                        OrgName = orgName,
                        PartnerUKPRN = delivery.LearningDelivery.PartnerUKPRN,
                        PartnerOrgName = partnerOrgName,
                        PrevUKPRN = delivery.Learner.PrevUKPRN,
                        PrevOrgName = prevOrgName,
                        PMUKPRN = delivery.Learner.PMUKPRN,
                        PMOrgName = pmOrgName,
                        ULN = delivery.Learner.ULN,
                        LearnRefNumber = delivery.Learner.LearnRefNumber,
                        SWSupAimId = delivery.LearningDelivery.SWSupAimId,
                        LearnAimRef = delivery.LearningDelivery.LearnAimRef,
                        LearnAimTitle = learnAim.LearnAimRefTitle,
                        AimSeqNumber = delivery.LearningDelivery.AimSeqNumber,
                        AimTypeCode = delivery.LearningDelivery.AimType,
                        LearnAimType = learnAim.LearnAimRefTypeDesc,
                        StdCode = delivery.LearningDelivery.StdCode,
                        FworkCode = delivery.LearningDelivery.FworkCode,
                        PwayCode = delivery.LearningDelivery.PwayCode,
                        ProgType = delivery.LearningDelivery.ProgType,
                        LearnStartDate = delivery.LearningDelivery.LearnStartDate,
                        OrigLearnStartDate = delivery.LearningDelivery.OrigLearnStartDate,
                        LearnPlanEndDate = delivery.LearningDelivery.LearnPlanEndDate,
                        LearnActEndDate = delivery.LearningDelivery.LearnActEndDate,
                        CompStatus = delivery.LearningDelivery.CompStatus,
                        Outcome = delivery.LearningDelivery.Outcome,
                        FundModel = delivery.LearningDelivery.FundModel,
                        SOFCode = sofCode,
                        AdvancedLoansIndicator = advancedLoansIndicator,
                        ResIndicator = resIndicator,
                        ProvSpecLearnDelMon = ProviderSpecDeliveryMonitorings(delivery.LearningDelivery.ProviderSpecDeliveryMonitorings),
                        ProvSpecDelMon = ProviderSpecLearningMonitorings(delivery.Learner.ProviderSpecLearnerMonitorings),
                        PriorLearnFundAdj = delivery.LearningDelivery.PriorLearnFundAdj,
                        OtherFundAdj = delivery.LearningDelivery.OtherFundAdj,
                        EPAOrgId = delivery.LearningDelivery.EPAOrgID,
                        TotalNegotiatedAssessmentPrice = aFinAmount,
                        AssessmentPaymentReceived = paymentsReceived
                    });
                }
            }

            return models;
        }

        private double DaysBetween(DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }
    }
}
