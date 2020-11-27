using System;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Worksheets
{
    public class Frm06ReportModel : BaseFrmReportModel
    {
        public string PrevOrgName { get; set; }

        public string PMOrgName { get; set; }

        public int AimTypeCode { get; set; }

        public string LearningAimType { get; set; }

        public int FundingModel { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public string SOFCode { get; set; }
    }
}
