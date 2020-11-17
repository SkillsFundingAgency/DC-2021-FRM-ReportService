using System;

namespace ESFA.DC.FRM.ReportService.Interfaces.ReferenceData
{
    public interface IMcaDevolvedContract
    {
        string McaGlaShortCode { get; }

        int Ukprn { get; }

        DateTime EffectiveFrom { get; }

        DateTime? EffectiveTo { get; }
    }
}
