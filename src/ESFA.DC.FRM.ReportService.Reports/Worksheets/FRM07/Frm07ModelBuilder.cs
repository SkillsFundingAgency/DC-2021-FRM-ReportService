using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Extensions;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM07
{
    public class Frm07ModelBuilder : BaseFrmModelBuilder, IModelBuilder<IEnumerable<Frm07ReportModel>>
    {
        private readonly int _pausedCompStatus = 6;
        private readonly int _fundModel99 = 99;
        private readonly string _fundModel99ADLCode = "1";

        private readonly HashSet<int> _excludedFundModel = new HashSet<int> { 25 };
        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public IEnumerable<Frm07ReportModel> Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var models = new List<Frm07ReportModel>();
            var returnPeriod = reportServiceContext.ReturnPeriodName;

            var organisationNameDictionary = reportData.Organisations.ToDictionary(k => k.Ukprn, v => v.Name);
            var learnAimDictionary = reportData.LARSLearningDeliveries.ToDictionary(k => k.LearnAimRef, v => v, StringComparer.OrdinalIgnoreCase);
            var sofCodeDictionary = reportData.McaGlaSofLookups.Where(l => DevolvedCodes.Contains(l.SofCode)).ToDictionary(k => k.SofCode, v => v.McaGlaShortCode);
            var mcaDictionary = reportData.McaDevolvedContracts.ToDictionary(k => k.McaGlaShortCode, v => v.Ukprn, StringComparer.OrdinalIgnoreCase);

            var orgName = organisationNameDictionary.GetValueOrDefault(reportServiceContext.Ukprn);

            var pausedDeliveries = reportData.Learners
                ?.SelectMany(l => l.LearningDeliveries.Where(ld =>
                        ld.CompStatus == _pausedCompStatus
                        && !ExcludedDelivery(ld, reportData.LARSLearningDeliveries)
                        && FundModel99Rule(ld)
                        && !_excludedFundModel.Contains(ld.FundModel))
                    .Select(ld => new { Learner = l, LearningDelivery = ld }));

            if (pausedDeliveries == null)
            {
                return models;
            }

            var learnerDeliveries = pausedDeliveries.GroupBy(x => x.Learner);

            foreach (var deliverySet in learnerDeliveries)
            {
                foreach (var delivery in deliverySet.OrderByDescending(x => x.LearningDelivery.LearnStartDate))
                {
                    var restartDelivery = GetRestartDelivery(delivery.LearningDelivery, delivery.Learner);

                    if (restartDelivery != null)
                    {
                        var pmOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PMUKPRN.GetValueOrDefault());
                        var prevOrgName = organisationNameDictionary.GetValueOrDefault(delivery.Learner.PrevUKPRN.GetValueOrDefault());

                        models.Add(BuildModelForLearningDelivery(reportServiceContext, restartDelivery, delivery.Learner, sofCodeDictionary, mcaDictionary, organisationNameDictionary, learnAimDictionary, returnPeriod, orgName, pmOrgName, prevOrgName));
                    }
                }
            }

            return models;
        }

        private Frm07ReportModel BuildModelForLearningDelivery(
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

            return new Frm07ReportModel
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
                PrevLearnRefNumber = learner.PrevLearnRefNumber,
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
                Outcome = learningDelivery.Outcome,
                FundModel = learningDelivery.FundModel,
                SOFCode = sofCode,
                AdvancedLoansIndicator = advancedLoansIndicator,
                ResIndicator = resIndicator,
                ProvSpecLearnDelMon = ProviderSpecDeliveryMonitorings(learningDelivery.ProviderSpecDeliveryMonitorings),
                ProvSpecDelMon = ProviderSpecLearningMonitorings(learner.ProviderSpecLearnerMonitorings),
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

        private LearningDelivery GetRestartDelivery(LearningDelivery breakLearningDelivery, Learner learner)
        {
            return learner.LearningDeliveries.FirstOrDefault(ld => ld.LearnPlanEndDate == breakLearningDelivery.LearnPlanEndDate
                                                             && ld.LearnAimRef.CaseInsensitiveEquals(breakLearningDelivery.LearnAimRef)
                                                             && ld.ProgType == breakLearningDelivery.ProgType
                                                             && ld.StdCode == breakLearningDelivery.StdCode
                                                             && ld.FworkCode == breakLearningDelivery.FworkCode
                                                             && HasRestartFAM(ld.LearningDeliveryFAMs)
                                                             && WithMatchingStartDates(breakLearningDelivery, ld));
        }

        private bool HasRestartFAM(ICollection<LearningDeliveryFAM> learningDeliveryFams)
        {
            return learningDeliveryFams?.Any(f => f.LearnDelFAMType.CaseInsensitiveEquals(RESLearnDelFamType)) ?? false;
        }

        private bool FundModel99Rule(LearningDelivery delivery)
        {
            return delivery.FundModel != _fundModel99 || RetrieveFamCodeForType(delivery.LearningDeliveryFAMs, ADLLearnDelFamType) == _fundModel99ADLCode;
        }

        private bool WithMatchingStartDates(LearningDelivery breakLearningDelivery, LearningDelivery learningDelivery)
        {
            if (learningDelivery.OrigLearnStartDate == null)
            {
                return false;
            }

            return (learningDelivery.OrigLearnStartDate == breakLearningDelivery.LearnStartDate
                    || learningDelivery.OrigLearnStartDate == breakLearningDelivery.OrigLearnStartDate)
                   && learningDelivery.LearnStartDate >= breakLearningDelivery.LearnActEndDate;
        }
    }
}
