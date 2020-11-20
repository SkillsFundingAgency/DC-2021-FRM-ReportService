using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;
using ESFA.DC.FRM.ReportService.Reports.Constants;
using ESFA.DC.FRM.ReportService.Reports.Extensions;
using ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06
{
    public class Frm06ModelBuilder : BaseFrmModelBuilder, IModelBuilder<IEnumerable<Frm06ReportModel>>
    {
        private readonly IEqualityComparer<LearnerKey> _frmEqualityComparer;

        private readonly int _excludedFundModel = 99;
        private readonly string _excludedFAMType = "ADL";
        private readonly string _excludedFAMCode = "1";
        private readonly HashSet<int> _excludedCategories = new HashSet<int> { 23, 24, 27, 28, 29, 34, 35, 36 };

        public Frm06ModelBuilder(IEqualityComparer<LearnerKey> frmEqualityComparer)
        {
            _frmEqualityComparer = frmEqualityComparer;
        }

        public IEnumerable<Frm06ReportModel> Build(IReportServiceContext reportServiceContext, IReportData reportData)
        {
            var models = new List<Frm06ReportModel>();

            var organisationNameDictionary = reportData.Organisations.ToDictionary(x => x.Ukprn, x => x.Name);
            var learnAimDictionary = reportData.LARSLearningDeliveries.ToDictionary(x => x.LearnAimRef, x => x, StringComparer.OrdinalIgnoreCase);

            var currentLearnersHashSet = BuildCurrentYearLearnerHashSet(reportData.Learners);

            var returnPeriod = reportServiceContext.ReturnPeriodName;

            foreach (var learner in FilterLearners(reportData))
            {
                var key = new LearnerKey
                {
                    FworkCodeNullable = learner.FworkCodeNullable,
                    LearnAimRef = learner.LearnAimRef,
                    LearnRefNumber = learner.LearnRefNumber,
                    LearnStartDate = learner.LearnStartDate,
                    ProgTypeNullable = learner.ProgTypeNullable
                };

                if (!currentLearnersHashSet.Contains(key))
                {
                    var advancedLoansIndicator = RetrieveFamCodeForType(learner.LearningDeliveryFAMs, ADLLearnDelFamType);
                    var resIndicator = RetrieveFamCodeForType(learner.LearningDeliveryFAMs, RESLearnDelFamType);
                    var sofCode = RetrieveFamCodeForType(learner.LearningDeliveryFAMs, SOFLearnDelFamType);
                    var pmOrgName = organisationNameDictionary.GetValueOrDefault(Convert.ToInt32(learner.PMUKPRN.GetValueOrDefault()));
                    var prevOrgName = organisationNameDictionary.GetValueOrDefault(Convert.ToInt32(learner.PrevUKPRN.GetValueOrDefault()));
                    var learnAim = learnAimDictionary.GetValueOrDefault(learner.LearnAimRef);

                    models.Add(new Frm06ReportModel
                    {
                        UKPRN = learner.UKPRN,
                        PrevOrgName = prevOrgName,
                        PMOrgName = pmOrgName,
                        AimTypeCode = learner.AimType,
                        LearningAimType = learnAim?.LearnAimRefTypeDesc,
                        FundingModel = learner.FundModel,
                        OrigLearnStartDate = learner.OrigLearnStartDate,
                        SOFCode = sofCode,
                        Return = returnPeriod,
                        OrgName = learner.OrgName,
                        FworkCode = learner.FworkCodeNullable,
                        LearnAimTitle = learnAim?.LearnAimRefTitle,
                        LearnAimRef = learner.LearnAimRef,
                        LearnRefNumber = learner.LearnRefNumber,
                        LearnStartDate = learner.LearnStartDate,
                        ProgType = learner.ProgTypeNullable,
                        StdCode = learner.StdCodeNullable,
                        ULN = learner.ULN,
                        AdvancedLoansIndicator = advancedLoansIndicator,
                        AimSeqNumber = learner.AimSeqNumber,
                        CompStatus = learner.CompStatus,
                        LearnActEndDate = learner.LearnActEndDate,
                        LearnPlanEndDate = learner.LearnPlanEndDate,
                        OtherFundAdj = learner.OtherFundAdj,
                        Outcome = learner.Outcome,
                        PMUKPRN = learner.PMUKPRN,
                        PartnerUKPRN = learner.PartnerUKPRN,
                        PartnerOrgName = learner.PartnerOrgName,
                        PriorLearnFundAdj = learner.PriorLearnFundAdj,
                        PrevUKPRN = learner.PrevUKPRN,
                        PwayCode = learner.PwayCodeNullable,
                        ResIndicator = resIndicator,
                        SWSupAimId = learner.SWSupAimId,
                        ProvSpecLearnDelMon = BuildProvSpecLearnDelMons(learner?.ProvSpecDeliveryMonitorings),
                        ProvSpecDelMon = BuildProvSpecLearnDelMons(learner?.ProviderSpecLearnerMonitorings)
                    });
                }
            }

            return models?.Where(LearningDeliveryFilter);
        }

        public bool LearningDeliveryFilter(Frm06ReportModel model)
        {
            if (model.FundingModel != FundModelConstants.FM99)
            {
                return true;
            }

            return model.FundingModel == FundModelConstants.FM99 && model.AdvancedLoansIndicator == LearningDeliveryFAMCodeConstants.ADL_1;
        }

        private IEnumerable<IPreviousYearLearner> FilterLearners(IReportData reportData)
        {
            foreach (var learner in reportData.PreviousYearLearners ?? Array.Empty<PreviousYearLearner>())
            {
                var excluded = reportData.LARSLearningDeliveries
                    .Any(x => string.Equals(x.LearnAimRef, learner.LearnAimRef, StringComparison.OrdinalIgnoreCase)
                              && x.LARSLearningDeliveryCategories.Any(ldc => _excludedCategories.Contains(ldc.CategoryRef)))
                    || FM99Exclusion(learner);

                if (!excluded)
                {
                    yield return learner;
                }
            }
        }

        private bool FM99Exclusion(IPreviousYearLearner learningDelivery) =>
            learningDelivery.FundModel == _excludedFundModel && learningDelivery.LearningDeliveryFAMs.Any(ldf => ldf.LearnDelFAMCode == _excludedFAMCode && ldf.LearnDelFAMType == _excludedFAMType);

        private string BuildProvSpecLearnDelMons(IReadOnlyCollection<IProviderSpecDeliveryMonitoring> monitorings)
        {
            return monitorings == null ? null : string.Join(";", monitorings.Select(x => x.ProvSpecDelMon));
        }

        private string BuildProvSpecLearnDelMons(IReadOnlyCollection<IProviderSpecLearnerMonitoring> monitorings)
        {
            return monitorings == null ? null : string.Join(";", monitorings.Select(x => x.ProvSpecLearnMon));
        }

        private HashSet<LearnerKey> BuildCurrentYearLearnerHashSet(IReadOnlyCollection<Learner> learners)
        {
            HashSet<LearnerKey> learnerKeys = new HashSet<LearnerKey>(_frmEqualityComparer);
            learnerKeys.UnionWith(learners?
               .SelectMany(l => l?.LearningDeliveries?
               .Select(ld => new LearnerKey
               {
                   FworkCodeNullable = ld.FworkCode,
                   LearnAimRef = ld.LearnAimRef,
                   LearnRefNumber = l.LearnRefNumber,
                   LearnStartDate = ld.LearnStartDate,
                   ProgTypeNullable = ld.ProgType
               })) ?? Enumerable.Empty<LearnerKey>());
            learnerKeys.UnionWith(learners?
               .Where(l => !string.IsNullOrEmpty(l.PrevLearnRefNumber))
               .SelectMany(l => l?.LearningDeliveries?
               .Select(ld => new LearnerKey
               {
                   FworkCodeNullable = ld.FworkCode,
                   LearnAimRef = ld.LearnAimRef,
                   LearnRefNumber = l.PrevLearnRefNumber,
                   LearnStartDate = ld.LearnStartDate,
                   ProgTypeNullable = ld.ProgType
               })) ?? Enumerable.Empty<LearnerKey>());

            return learnerKeys;
        }
    }
}