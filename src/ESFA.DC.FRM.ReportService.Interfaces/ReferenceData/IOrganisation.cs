using System;
using System.Collections.Generic;
using System.Text;

namespace ESFA.DC.FRM.ReportService.Interfaces.ReferenceData
{
    public interface IOrganisation
    {
        long Ukprn { get; }

        string Name { get; }
    }
}
