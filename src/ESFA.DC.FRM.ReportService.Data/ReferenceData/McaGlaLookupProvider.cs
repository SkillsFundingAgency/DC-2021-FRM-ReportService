using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class McaGlaLookupProvider : IDataProvider<IMcaGlaSofLookup>
    {
        private readonly Func<SqlConnection> _sqlConnectionFunc;

        private readonly string _sql = @"SELECT
	                                        sof.SofCode,
	                                        sof.McaGlaShortCode,
	                                        COALESCE(fn.FullName, '') AS McaGlaFullName
                                        FROM [dbo].[MCAGLA_SOF] sof
                                        LEFT JOIN [dbo].[MCAGLA_FullName] fn
	                                        ON sof.McaglaShortCode = fn.McaglaShortCode
                                        WHERE fn.EffectiveTo IS NULL";

        public McaGlaLookupProvider(Func<SqlConnection> sqlConnectionFunc)
        {
            _sqlConnectionFunc = sqlConnectionFunc;
        }

        public async Task<IReadOnlyCollection<IMcaGlaSofLookup>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var connection = _sqlConnectionFunc())
            {
                return (await connection.QueryAsync<McaGlaSofLookup>(_sql, cancellationToken)).ToList();
            }
        }
    }
}
