using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Constants;
using ESFA.DC.FRM.ReportService.Reports.Extensions;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.ILR.Model.Interface;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM09
{
    public class Frm09ModelBuilder : BaseFrmModelBuilder, IModelBuilder<IEnumerable<Frm09ReportModel>>
    {
        private readonly int _withdrawnCompStatus = 3;
        private readonly int _withdrawnReasonCode = 40;
        private readonly int _excludedAimType = 3;
        private readonly int _fundModel99 = 99;
        private readonly string _fundModel99ADLCode = "1";

        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public IEnumerable<Frm09ReportModel> Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var models = new List<Frm09ReportModel>();
            var returnPeriod = reportServiceContext.ReturnPeriodName;

            var organisationNameDictionary = reportData.Organisations.ToDictionary(k => k.Ukprn, v => v.Name);
            var learnAimDictionary = reportData.LARSLearningDeliveries.ToDictionary(k => k.LearnAimRef, v => v, StringComparer.OrdinalIgnoreCase);
            var sofCodeDictionary = reportData.McaGlaSofLookups.Where(l => DevolvedCodes.Contains(l.SofCode)).ToDictionary(k => k.SofCode, v => v.McaGlaShortCode);
            var mcaDictionary = reportData.McaDevolvedContracts.ToDictionary(k => k.McaGlaShortCode, v => v.Ukprn,
                StringComparer.OrdinalIgnoreCase);

            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            var currentReturnEndDate = reportData.ReturnPeriods
                .FirstOrDefault(d => d.Period == reportServiceContext.ReturnPeriod).End;

            var withdrawanDeliveries = reportData.Learners
                ?.SelectMany(l => l.LearningDeliveries.Where(ld =>
                        ld.CompStatus == _withdrawnCompStatus
                        && ld.WithdrawReasonNullable == _withdrawnReasonCode
                        && !ExcludedDelivery(ld, reportData.LARSLearningDeliveries)
                        && FundModel99Rule(ld)
                        && ld.LearnActEndDateNullable <= ReportingConstants.EndOfYear)
                    .Select(ld => new { Learner = l, LearningDelivery = ld }));

            if (withdrawanDeliveries == null)
            {
                return models;
            }

            foreach (var delivery in withdrawanDeliveries)
            {
                if (!HasRestartDelivery(delivery.LearningDelivery, delivery.Learner, reportData.Learners))
                {
                    if (delivery.LearningDelivery.LearnActEndDateNullable != null && DaysBetween(delivery.LearningDelivery.LearnActEndDateNullable.Value, currentReturnEndDate) >= 90)
                    {
                        var pmOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PMUKPRNNullable.GetValueOrDefault());
                        var prevOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PrevUKPRNNullable.GetValueOrDefault());

                        models.Add(BuildModelForLearningDelivery(reportServiceContext, delivery.LearningDelivery, delivery.Learner, sofCodeDictionary, mcaDictionary, organisationNameDictionary, learnAimDictionary, returnPeriod, orgName, pmOrgName, prevOrgName));
                    }
                }
            }

            return models;
        }

        private Frm09ReportModel BuildModelForLearningDelivery(IReportServiceContext reportServiceContext, ILearningDelivery learningDelivery, ILearner learner,
            Dictionary<string, string> sofCodeDictionary, Dictionary<string, int> mcaDictionary, Dictionary<long, string> organisationNameDictionary,
            Dictionary<string, ILARSLearningDelivery> learnAimDictionary, string returnPeriod, string orgName, string pmOrgName, string prevOrgName)
        {
            var advancedLoansIndicator = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, ADLLearnDelFamType);
            var resIndicator = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, RESLearnDelFamType);

            var sofCode = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, SOFLearnDelFamType);
            var mcaShortCode = sofCodeDictionary.GetValueOrDefault(sofCode);
            var sofUkprn = mcaDictionary.GetValueOrDefault(mcaShortCode);

            var partnerOrgName = organisationNameDictionary.GetValueOrDefault(learningDelivery.PartnerUKPRNNullable.GetValueOrDefault());
            var learningAim = learnAimDictionary.GetValueOrDefault(learningDelivery.LearnAimRef);

            var sofOrgName = organisationNameDictionary.GetValueOrDefault(sofUkprn);

            return new Frm09ReportModel
            {
                Return = returnPeriod,
                UKPRN = reportServiceContext.Ukprn,
                OrgName = orgName,
                PartnerUKPRN = learningDelivery.PartnerUKPRNNullable,
                PartnerOrgName = partnerOrgName,
                PrevUKPRN = learner.PrevUKPRNNullable,
                PrevOrgName = prevOrgName,
                PMUKPRN = learner.PMUKPRNNullable,
                PMOrgName = pmOrgName,
                DevolvedUKPRN = sofUkprn != 0 ? sofUkprn : (int?)null,
                DevolvedOrgName = sofOrgName,
                ULN = learner.ULN,
                LearnRefNumber = learner.LearnRefNumber,
                SWSupAimId = learningDelivery.SWSupAimId,
                LearnAimRef = learningDelivery.LearnAimRef,
                LearnAimTitle = learningAim.LearnAimRefTitle,
                AimSeqNumber = learningDelivery.AimSeqNumber,
                AimTypeCode = learningDelivery.AimType,
                LearnAimType = learningAim.LearnAimRefTypeDesc,
                StdCode = learningDelivery.StdCodeNullable,
                FworkCode = learningDelivery.FworkCodeNullable,
                PwayCode = learningDelivery.PwayCodeNullable,
                ProgType = learningDelivery.ProgTypeNullable,
                LearnStartDate = learningDelivery.LearnStartDate,
                OrigLearnStartDate = learningDelivery.OrigLearnStartDateNullable,
                LearnPlanEndDate = learningDelivery.LearnPlanEndDate,
                LearnActEndDate = learningDelivery.LearnActEndDateNullable,
                CompStatus = learningDelivery.CompStatus,
                WithdrawalCode = learningDelivery.WithdrawReasonNullable,
                Outcome = learningDelivery.OutcomeNullable,
                FundModel = learningDelivery.FundModel,
                SOFCode = sofCode,
                AdvancedLoansIndicator = advancedLoansIndicator,
                ResIndicator = resIndicator,
                ProvSpecLearnDelMon = ProviderSpecLearningMonitorings(learner.ProviderSpecLearnerMonitorings),
                ProvSpecDelMon = ProviderSpecDeliveryMonitorings(learningDelivery.ProviderSpecDeliveryMonitorings),
                PriorLearnFundAdj = learningDelivery.PriorLearnFundAdjNullable,
                OtherFundAdj = learningDelivery.OtherFundAdjNullable,
            };
        }

        private bool ExcludedDelivery(ILearningDelivery learner, IReadOnlyCollection<ILARSLearningDelivery> larsLearningDeliveries)
        {
            return larsLearningDeliveries
                .Any(x => x.LearnAimRef.CaseInsensitiveEquals(learner.LearnAimRef)
                          && x.LARSLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)));
        }

        private bool FundModel99Rule(ILearningDelivery delivery)
        {
            return delivery.FundModel != _fundModel99 || RetrieveFamCodeForType(delivery.LearningDeliveryFAMs, ADLLearnDelFamType) == _fundModel99ADLCode;
        }

        private bool HasRestartDelivery(ILearningDelivery withdrawnLearningDelivery, ILearner withdrawnLearner, IReadOnlyCollection<ILearner> learners)
        {
            return learners.Where(l =>
                    (l.LearnRefNumber == withdrawnLearner.LearnRefNumber) ||
                    (l.PrevLearnRefNumber == withdrawnLearner.LearnRefNumber))
                .SelectMany(l => l.LearningDeliveries.Where(ld =>
                    ld.ProgTypeNullable == withdrawnLearningDelivery.ProgTypeNullable
                    && ld.StdCodeNullable == withdrawnLearningDelivery.StdCodeNullable
                    && ld.FworkCodeNullable == withdrawnLearningDelivery.FworkCodeNullable
                    && ld.LearnStartDate >= withdrawnLearningDelivery.LearnActEndDateNullable)).Any();
        }

        public double DaysBetween(DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }
    }
}
