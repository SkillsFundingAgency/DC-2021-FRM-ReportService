using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;
using ESFA.DC.FRM.ReportService.Reports.Model.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Data.ReferenceData
{
    public class ReturnPeriodProvider : IDataProvider<IReturnPeriod>
    {
        public async Task<IReadOnlyCollection<IReturnPeriod>> ProvideAsync(IReportServiceContext reportServiceContext, CancellationToken cancellationToken)
        {
            var returnPeriods = new List<IReturnPeriod>
            {
                new ReturnPeriod { Name = "R01", Period = 1, Start = new DateTime(2019, 08, 01), End = new DateTime(2020, 09, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R02", Period = 2, Start = new DateTime(2019, 09, 05), End = new DateTime(2020, 10, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R03", Period = 3, Start = new DateTime(2019, 10, 07), End = new DateTime(2020, 11, 05, 23, 59, 59) },
                new ReturnPeriod { Name = "R04", Period = 4, Start = new DateTime(2019, 11, 06), End = new DateTime(2020, 12, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R05", Period = 5, Start = new DateTime(2019, 12, 05), End = new DateTime(2021, 01, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R06", Period = 6, Start = new DateTime(2020, 01, 08), End = new DateTime(2021, 02, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R07", Period = 7, Start = new DateTime(2020, 02, 05), End = new DateTime(2021, 03, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R08", Period = 8, Start = new DateTime(2020, 03, 05), End = new DateTime(2021, 04, 08, 23, 59, 59) },
                new ReturnPeriod { Name = "R09", Period = 9, Start = new DateTime(2020, 04, 07), End = new DateTime(2021, 05, 07, 23, 59, 59) },
                new ReturnPeriod { Name = "R10", Period = 10, Start = new DateTime(2020, 05, 08), End = new DateTime(2021, 06, 04, 23, 59, 59) },
                new ReturnPeriod { Name = "R11", Period = 11, Start = new DateTime(2020, 06, 05), End = new DateTime(2021, 07, 06, 23, 59, 59) },
                new ReturnPeriod { Name = "R12", Period = 12, Start = new DateTime(2020, 07, 07), End = new DateTime(2021, 08, 05, 23, 59, 59) },
                new ReturnPeriod { Name = "R13", Period = 13, Start = new DateTime(2020, 08, 06), End = new DateTime(2021, 09, 14, 23, 59, 59) },
                new ReturnPeriod { Name = "R14", Period = 14, Start = new DateTime(2020, 09, 15), End = new DateTime(2021, 10, 21, 23, 59, 59) }
            };

            return await Task.FromResult(returnPeriods);
        }
    }
}
