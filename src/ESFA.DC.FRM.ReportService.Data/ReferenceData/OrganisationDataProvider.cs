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
    public class OrganisationDataProvider : IDataProvider<IOrganisation>
    {
        private readonly Func<SqlConnection> _sqlConnectionFunc;
        private readonly string _sql = @"SELECT [UKPRN]
                                              ,[Name]
                                          FROM [dbo].[Org_Details]";

        public OrganisationDataProvider(Func<SqlConnection> sqlConnectionFunc)
        {
            _sqlConnectionFunc = sqlConnectionFunc;
        }

        public async Task<IReadOnlyCollection<IOrganisation>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            using (var connection = _sqlConnectionFunc())
            {
                return (await connection.QueryAsync<Organisation>(_sql, cancellationToken)).ToList<IOrganisation>();
            }
        }
    }
}
