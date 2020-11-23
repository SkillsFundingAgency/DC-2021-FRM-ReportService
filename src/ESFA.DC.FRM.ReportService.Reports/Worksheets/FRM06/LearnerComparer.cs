using System.Collections.Generic;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06
{
    public class LearnerComparer : IEqualityComparer<LearnerKey>
    {
        public bool Equals(LearnerKey x, LearnerKey y)
        {
            return GetHashCode(x) == GetHashCode(y);
        }

        public int GetHashCode(LearnerKey obj)
        {
            if (obj == null)
            {
                return 0;
            }

            return (
                obj.LearnRefNumber = obj.LearnRefNumber.ToLowerInvariant(),
                obj.FworkCodeNullable = obj.FworkCodeNullable,
                obj.LearnAimRef = obj.LearnAimRef.ToLowerInvariant(),
                obj.LearnStartDate = obj.LearnStartDate,
                obj.ProgTypeNullable = obj.ProgTypeNullable,
                obj.StdCodeNullable = obj.StdCodeNullable).GetHashCode();
        }
    }
}
