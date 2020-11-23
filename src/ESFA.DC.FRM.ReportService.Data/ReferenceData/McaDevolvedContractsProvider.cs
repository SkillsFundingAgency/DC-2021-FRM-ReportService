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
    public class McaDevolvedContractsProvider : IDataProvider<IMcaDevolvedContract>
    {
        private readonly Func<SqlConnection> _sqlConnectionFunc;
        private readonly string _sql = @"SELECT
	                                        UKPRN,
	                                        McaGlaShortCode,
	                                        EffectiveFrom,
	                                        EffectiveTo
                                        FROM [dbo].[DevolvedContract]
                                        WHERE UKPRN = @Ukprn";

        public McaDevolvedContractsProvider(Func<SqlConnection> sqlConnectionFunc)
        {
            _sqlConnectionFunc = sqlConnectionFunc;
        }

        public async Task<IReadOnlyCollection<IMcaDevolvedContract>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var connection = _sqlConnectionFunc())
            {
                return (await connection.QueryAsync<McaDevolvedContract>(_sql, new { reportServiceContext.Ukprn })).ToList();
            }
        }
    }
}
