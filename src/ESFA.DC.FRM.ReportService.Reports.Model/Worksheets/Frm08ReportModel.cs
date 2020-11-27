using System;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Worksheets
{
    public class Frm08ReportModel : BaseFrmReportModel
    {
        public string LearnAimType { get; set; }

        public int FundModel { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public string SOFCode { get; set; }

        public int AimTypeCode { get; set; }
    }
}
