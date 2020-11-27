using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;
using ESFA.DC.FRM.ReportService.Interfaces.PreviousYear;
using ESFA.DC.FRM.ReportService.Reports.Model.PreviousYear;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class PreviousYearLearnerDataProvider : IDataProvider<IPreviousYearLearner>
    {
        private readonly DateTime _currentYearStart = new DateTime(2020, 8, 1);
        private readonly Func<SqlConnection> _sqlConnectionFunc;

        private readonly string _sql = @"SELECT
	                                        [l].UKPRN,
	                                        [l].ULN,
	                                        [ld].AimType,
	                                        [ld].FundModel,
	                                        [ld].FworkCode,
	                                        [ld].LearnAimRef,
	                                        [ld].LearnRefNumber,
	                                        [ld].LearnStartDate,
	                                        [ld].ProgType,
	                                        [ld].StdCode,
	                                        [ld].PwayCode,
	                                        [ld].AimSeqNumber,
	                                        [ld].PartnerUKPRN,
	                                        [l].PrevUKPRN,
                                            [l].PMUKPRN,
                                            [l].PrevLearnRefNumber,
                                            [ld].CompStatus,
                                            [ld].Outcome,
                                            [ld].PriorLearnFundAdj,
                                            [ld].OtherFundAdj,
                                            [ld].LearnPlanEndDate,
                                            [ld].LearnActEndDate,
                                            [ld].SWSupAimId,
                                            [ld].OrigLearnStartDate
                                        FROM [Valid].[Learner] l
                                        INNER JOIN [Valid].[LearningDelivery] ld
	                                        ON l.UKPRN = ld.UKPRN
	                                        AND l.LearnRefNumber = ld.LearnRefNumber
                                        WHERE 
                                        [l].UKPRN = @ukprn
                                        AND [ld].CompStatus = 1
                                        AND [ld].LearnPlanEndDate >= @currentYearStart;

                                        SELECT
	                                        LearnRefNumber,
	                                        AimSeqNumber,
	                                        LearnDelFAMCode,
                                            LearnDelFAMType,
                                            LearnDelFAMDateFrom,
                                            LearnDelFAMDateTo
                                        FROM [Valid].[LearningDeliveryFAM]
                                        WHERE UKPRN = @ukprn;

                                        SELECT
	                                        LearnRefNumber,
	                                        ProvSpecLearnMon,
                                            ProvSpecLearnMonOccur
                                        FROM [Valid].[ProviderSpecLearnerMonitoring]
                                        WHERE UKPRN = @ukprn;

                                        SELECT
	                                        LearnRefNumber,
	                                        AimSeqNumber,
	                                        ProvSpecDelMon,
                                            ProvSpecDelMonOccur
                                        FROM [Valid].[ProviderSpecDeliveryMonitoring]
                                        WHERE UKPRN = @ukprn;";

        public PreviousYearLearnerDataProvider(Func<SqlConnection> sqlConnectionFunc)
        {
            _sqlConnectionFunc = sqlConnectionFunc;
        }

        public async Task<IReadOnlyCollection<IPreviousYearLearner>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var connection = _sqlConnectionFunc())
            {
                using (var results = await connection.QueryMultipleAsync(_sql, new { ukprn = reportServiceContext.Ukprn, currentYearStart = _currentYearStart }))
                {
                    var learners = (await results.ReadAsync<PreviousYearLearner>()).ToList();
                    var learnDelFAMS = (await results.ReadAsync<LearningDeliveryFAM>()).GroupBy(x => x.LearnRefNumber, StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, x => x.GroupBy(y => y.AimSeqNumber).ToDictionary(d => d.Key, d => d));
                    var provSpecLearnMon = (await results.ReadAsync<ProviderSpecLearnerMonitoring>()).GroupBy(x => x.LearnRefNumber, StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, x => x, StringComparer.OrdinalIgnoreCase);
                    var provSpecDelMon = (await results.ReadAsync<ProviderSpecDeliveryMonitoring>()).GroupBy(x => x.LearnRefNumber, StringComparer.OrdinalIgnoreCase).ToDictionary(x => x.Key, x => x.GroupBy(y => y.AimSeqNumber).ToDictionary(d => d.Key, d => d));

                    foreach (var learner in learners)
                    {
                        learner.LearningDeliveryFAMs = learnDelFAMS.GetValueOrDefault(learner.LearnRefNumber)
                            .GetValueOrDefault(learner.AimSeqNumber).ToList();

                        learner.ProviderSpecLearnerMonitorings = provSpecLearnMon.GetValueOrDefault(learner.LearnRefNumber).ToList();

                        learner.ProvSpecDeliveryMonitorings = provSpecDelMon.GetValueOrDefault(learner.LearnRefNumber)
                            .GetValueOrDefault(learner.AimSeqNumber).ToList();
                    }

                    return learners.ToList();
                }
            }
        }
    }
}
