using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM08
{
    public class Frm08ModelBuilder : BaseFrmModelBuilder, IModelBuilder<IEnumerable<Frm08ReportModel>>
    {
        private readonly int _pausedCompStatus = 6;
        private readonly int _fundModel99 = 99;
        private readonly string _fundModel99ADLCode = "1";
        private readonly HashSet<int> _excludedFundModel = new HashSet<int> { 25 };
        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public IEnumerable<Frm08ReportModel> Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var models = new List<Frm08ReportModel>();
            var returnPeriod = reportServiceContext.ReturnPeriodName;

            var organisationNameDictionary = reportData.Organisations.ToDictionary(x => x.Ukprn, x => x.Name);

            var learnAimDictionary = reportData.LARSLearningDeliveries.ToDictionary(x => x.LearnAimRef, x => x, StringComparer.OrdinalIgnoreCase);

            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            var pausedDeliveries = reportData.Learners
                                        ?.SelectMany(l => l.LearningDeliveries.Where(ld =>
                                            ld.CompStatus == _pausedCompStatus
                                            && !ExcludedDelivery(ld, reportData.LARSLearningDeliveries)
                                            && !_excludedFundModel.Contains(ld.FundModel)
                                            && FundModel99Rule(ld))
                                            .Select(ld => new { Learner = l, LearningDelivery = ld }));

            var currentReturnEndDate = reportData.ReturnPeriods.FirstOrDefault(d =>
                                                                                    reportServiceContext.SubmissionDateTimeUtc >= d.Start
                                                                                    && reportServiceContext.SubmissionDateTimeUtc <= d.End).End;

            if (pausedDeliveries == null)
            {
                return Enumerable.Empty<Frm08ReportModel>();
            }

            foreach (var delivery in pausedDeliveries)
            {
                var restartDelivery = GetRestartDelivery(delivery.LearningDelivery, delivery.Learner);

                if (restartDelivery != null)
                {
                    continue;
                }

                var learnActEndDate = delivery.LearningDelivery.LearnActEndDate;

                if (learnActEndDate.HasValue && DaysBetween(learnActEndDate.Value, currentReturnEndDate) >= 365)
                {
                    var advancedLoansIndicator = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, ADLLearnDelFamType);
                    var sofCode = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, SOFLearnDelFamType);
                    var resIndicator = RetrieveFamCodeForType(delivery.LearningDelivery.LearningDeliveryFAMs, RESLearnDelFamType);

                    var pmOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PMUKPRN.GetValueOrDefault());
                    var prevOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PrevUKPRN.GetValueOrDefault());
                    var partnerOrgName = organisationNameDictionary.GetValueOrDefault(delivery.LearningDelivery.PartnerUKPRN.GetValueOrDefault());
                    var learnAim = learnAimDictionary.GetValueOrDefault(delivery.LearningDelivery.LearnAimRef);

                    models.Add(new Frm08ReportModel
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
                    });
                }
            }

            return models;
        }

        private LearningDelivery GetRestartDelivery(LearningDelivery breakLearningDelivery, Learner learner)
        {
            return learner?.LearningDeliveries?.FirstOrDefault(ld => ld.LearnAimRef.CaseInsensitiveEquals(breakLearningDelivery.LearnAimRef)
                                                                   && ld.ProgType == breakLearningDelivery.ProgType
                                                                   && ld.StdCode == breakLearningDelivery.StdCode
                                                                   && ld.FworkCode == breakLearningDelivery.FworkCode
                                                                   && HasRestartFAM(ld.LearningDeliveryFAMs)
                                                                   && WithMatchingStartDates(breakLearningDelivery, ld));
        }

        private bool HasRestartFAM(ICollection<LearningDeliveryFAM> learningDeliveryFams)
        {
            return learningDeliveryFams?.Any(f => f.LearnDelFAMType.Equals(RESLearnDelFamType, StringComparison.OrdinalIgnoreCase))
                ?? false;
        }

        private bool FundModel99Rule(LearningDelivery delivery)
        {
            return delivery.FundModel != _fundModel99 || RetrieveFamCodeForType(delivery.LearningDeliveryFAMs, ADLLearnDelFamType) == _fundModel99ADLCode;
        }

        private bool WithMatchingStartDates(LearningDelivery breakLearningDelivery, LearningDelivery learningDelivery)
        {
            if (learningDelivery?.OrigLearnStartDate == null)
            {
                return false;
            }

            return (learningDelivery.OrigLearnStartDate.Value == breakLearningDelivery.LearnStartDate
                || learningDelivery.OrigLearnStartDate.Value == breakLearningDelivery.OrigLearnStartDate)
                && learningDelivery.LearnStartDate >= breakLearningDelivery.LearnActEndDate;
        }

        private bool ExcludedDelivery(LearningDelivery learner, IReadOnlyCollection<ILARSLearningDelivery> larsLearningDeliveries)
        {
            return larsLearningDeliveries?
                .Any(x => x.LearnAimRef.CaseInsensitiveEquals(learner.LearnAimRef)
                          && x.LARSLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)))
                ?? false;
        }

        private double DaysBetween(DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }
    }
}
