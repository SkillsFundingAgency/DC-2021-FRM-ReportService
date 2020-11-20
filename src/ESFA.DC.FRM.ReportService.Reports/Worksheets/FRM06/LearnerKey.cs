using System;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06
{
    public class LearnerKey
    {
        public string LearnRefNumber { get; set; }

        public string LearnAimRef { get; set; }

        public int? ProgTypeNullable { get; set; }

        public int? StdCodeNullable { get; set; }

        public int? FworkCodeNullable { get; set; }

        public DateTime LearnStartDate { get; set; }
    }
}
