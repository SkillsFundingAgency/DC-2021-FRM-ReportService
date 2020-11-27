using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class LARSLearningDeliveryProvider : IDataProvider<ILARSLearningDelivery>
    {
        private readonly Func<SqlConnection> _sqlConnectionFunc;
        private readonly string _sql = @"SELECT
	                                        LearnAimRef,
	                                        LearnAimRefTitle,
	                                        LearnAimRefTypeDesc
                                        FROM [Core].[LARS_LearningDelivery] ld
                                        INNER JOIN [CoreReference].[LARS_LearnAimRefType_Lookup] lart
	                                        ON ld.LearnAimRefType = lart.LearnAimRefType";

        private readonly string _categorySql = @"SELECT
	                                                LearnAimRef,
	                                                CategoryRef,
	                                                EffectiveFrom,
	                                                EffectiveTo
                                                FROM [Core].[LARS_LearningDeliveryCategory]
                                                WHERE LearnAimRef = @learnAimRef";

        public LARSLearningDeliveryProvider(Func<SqlConnection> sqlConnectionFunc)
        {
            _sqlConnectionFunc = sqlConnectionFunc;
        }

        public async Task<IReadOnlyCollection<ILARSLearningDelivery>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var connection = _sqlConnectionFunc())
            {
                var larsDeliverys = (await connection.QueryAsync<LARSLearningDelivery>(_sql)).ToList();

                foreach (var delivery in larsDeliverys)
                {
                    delivery.LARSLearningDeliveryCategories = (await connection.QueryAsync<LARSLearningDeliveryCategory>(_sql, new { learnAimRef = delivery.LearnAimRef })).ToList();
                }

                return larsDeliverys;
            }
        }
    }
}
