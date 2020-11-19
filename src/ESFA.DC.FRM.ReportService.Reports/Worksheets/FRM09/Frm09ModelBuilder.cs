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
using ESFA.DC.ILR2021.DataStore.EF;

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
            var mcaDictionary = reportData.McaDevolvedContracts.ToDictionary(k => k.McaGlaShortCode, v => v.Ukprn, StringComparer.OrdinalIgnoreCase);

            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            var currentReturnEndDate = reportData.ReturnPeriods
                .FirstOrDefault(d => d.Period == reportServiceContext.ReturnPeriod).End;

            var withdrawanDeliveries = reportData.Learners
                ?.SelectMany(l => l.LearningDeliveries.Where(ld =>
                        ld.CompStatus == _withdrawnCompStatus
                        && ld.WithdrawReason == _withdrawnReasonCode
                        && !ExcludedDelivery(ld, reportData.LARSLearningDeliveries)
                        && FundModel99Rule(ld)
                        && ld.LearnActEndDate <= ReportingConstants.EndOfYear)
                    .Select(ld => new { Learner = l, LearningDelivery = ld }));

            if (withdrawanDeliveries == null)
            {
                return models;
            }

            foreach (var delivery in withdrawanDeliveries)
            {
                if (!HasRestartDelivery(delivery.LearningDelivery, delivery.Learner, reportData.Learners))
                {
                    if (delivery.LearningDelivery.LearnActEndDate != null && DaysBetween(delivery.LearningDelivery.LearnActEndDate.Value, currentReturnEndDate) >= 90)
                    {
                        var pmOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PMUKPRN.GetValueOrDefault());
                        var prevOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PrevUKPRN.GetValueOrDefault());

                        models.Add(BuildModelForLearningDelivery(reportServiceContext, delivery.LearningDelivery, delivery.Learner, sofCodeDictionary, mcaDictionary, organisationNameDictionary, learnAimDictionary, returnPeriod, orgName, pmOrgName, prevOrgName));
                    }
                }
            }

            return models;
        }

        private Frm09ReportModel BuildModelForLearningDelivery(
            IReportServiceContext reportServiceContext,
            LearningDelivery learningDelivery,
            Learner learner,
            Dictionary<string, string> sofCodeDictionary,
            Dictionary<string, int> mcaDictionary,
            Dictionary<long, string> organisationNameDictionary,
            Dictionary<string, ILARSLearningDelivery> learnAimDictionary,
            string returnPeriod,
            string orgName,
            string pmOrgName,
            string prevOrgName)
        {
            var advancedLoansIndicator = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, ADLLearnDelFamType);
            var resIndicator = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, RESLearnDelFamType);

            var sofCode = RetrieveFamCodeForType(learningDelivery.LearningDeliveryFAMs, SOFLearnDelFamType);
            var mcaShortCode = sofCodeDictionary.GetValueOrDefault(sofCode);
            var sofUkprn = mcaDictionary.GetValueOrDefault(mcaShortCode);

            var partnerOrgName = organisationNameDictionary.GetValueOrDefault(learningDelivery.PartnerUKPRN.GetValueOrDefault());
            var learningAim = learnAimDictionary.GetValueOrDefault(learningDelivery.LearnAimRef);

            var sofOrgName = organisationNameDictionary.GetValueOrDefault(sofUkprn);

            return new Frm09ReportModel
            {
                Return = returnPeriod,
                UKPRN = reportServiceContext.Ukprn,
                OrgName = orgName,
                PartnerUKPRN = learningDelivery.PartnerUKPRN,
                PartnerOrgName = partnerOrgName,
                PrevUKPRN = learner.PrevUKPRN,
                PrevOrgName = prevOrgName,
                PMUKPRN = learner.PMUKPRN,
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
                StdCode = learningDelivery.StdCode,
                FworkCode = learningDelivery.FworkCode,
                PwayCode = learningDelivery.PwayCode,
                ProgType = learningDelivery.ProgType,
                LearnStartDate = learningDelivery.LearnStartDate,
                OrigLearnStartDate = learningDelivery.OrigLearnStartDate,
                LearnPlanEndDate = learningDelivery.LearnPlanEndDate,
                LearnActEndDate = learningDelivery.LearnActEndDate,
                CompStatus = learningDelivery.CompStatus,
                WithdrawalCode = learningDelivery.WithdrawReason,
                Outcome = learningDelivery.Outcome,
                FundModel = learningDelivery.FundModel,
                SOFCode = sofCode,
                AdvancedLoansIndicator = advancedLoansIndicator,
                ResIndicator = resIndicator,
                ProvSpecLearnDelMon = ProviderSpecLearningMonitorings(learner.ProviderSpecLearnerMonitorings),
                ProvSpecDelMon = ProviderSpecDeliveryMonitorings(learningDelivery.ProviderSpecDeliveryMonitorings),
                PriorLearnFundAdj = learningDelivery.PriorLearnFundAdj,
                OtherFundAdj = learningDelivery.OtherFundAdj,
            };
        }

        private bool ExcludedDelivery(LearningDelivery learner, IReadOnlyCollection<ILARSLearningDelivery> larsLearningDeliveries)
        {
            return larsLearningDeliveries
                .Any(x => x.LearnAimRef.CaseInsensitiveEquals(learner.LearnAimRef)
                          && x.LARSLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)));
        }

        private bool FundModel99Rule(LearningDelivery delivery)
        {
            return delivery.FundModel != _fundModel99 || RetrieveFamCodeForType(delivery.LearningDeliveryFAMs, ADLLearnDelFamType) == _fundModel99ADLCode;
        }

        private bool HasRestartDelivery(LearningDelivery withdrawnLearningDelivery, Learner withdrawnLearner, IReadOnlyCollection<Learner> learners)
        {
            return learners.Where(l =>
                    (l.LearnRefNumber == withdrawnLearner.LearnRefNumber) ||
                    (l.PrevLearnRefNumber == withdrawnLearner.LearnRefNumber))
                .SelectMany(l => l.LearningDeliveries.Where(ld =>
                    ld.ProgType == withdrawnLearningDelivery.ProgType
                    && ld.StdCode == withdrawnLearningDelivery.StdCode
                    && ld.FworkCode == withdrawnLearningDelivery.FworkCode
                    && ld.LearnStartDate >= withdrawnLearningDelivery.LearnActEndDate)).Any();
        }

        private double DaysBetween(DateTime start, DateTime end)
        {
            return (end - start).TotalDays;
        }
    }
}
