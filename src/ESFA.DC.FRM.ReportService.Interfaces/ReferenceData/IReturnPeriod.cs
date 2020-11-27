using System;

namespace ESFA.DC.FRM.ReportService.Interfaces.ReferenceData
{
    public interface IReturnPeriod
    {
        string Name { get; }

        int Period { get; }

        DateTime Start { get; }

        DateTime End { get; }
    }
}
