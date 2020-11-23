using System;
using System.Collections.Generic;

namespace ESFA.DC.FRM.ReportService.Interfaces
{
    public interface IReportServiceContext
    {
        long JobId { get; }

        string Filename { get; }

        long Ukprn { get; }

        string Container { get; }

        IEnumerable<string> Tasks { get; }

        DateTime SubmissionDateTimeUtc { get; }

        int ReturnPeriod { get; }

        string ReturnPeriodName { get; }
    }
}