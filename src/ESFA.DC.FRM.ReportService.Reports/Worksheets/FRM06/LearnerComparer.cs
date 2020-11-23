using System.Collections.Generic;
using ESFA.DC.FRM.ReportService.Interfaces.Extensions;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06
{
    public class LearnerComparer : IEqualityComparer<LearnerKey>
    {
        public bool Equals(LearnerKey x, LearnerKey y)
        {
            if ((x == null && y != null) || (x != null && y == null))
            {
                return false;
            }

            if (x == null && y == null)
            {
                return true;
            }

            return x.LearnRefNumber.CaseInsensitiveEquals(y.LearnRefNumber)
                   && x.FworkCodeNullable == y.FworkCodeNullable
                   && x.LearnAimRef.CaseInsensitiveEquals(y.LearnAimRef)
                   && x.LearnStartDate == y.LearnStartDate
                   && x.ProgTypeNullable == y.ProgTypeNullable
                   && x.StdCodeNullable == y.StdCodeNullable;
        }

        public int GetHashCode(LearnerKey obj)
        {
            throw new System.NotImplementedException();
        }
    }
}
