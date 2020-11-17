using System;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Worksheets
{
    public class Frm09ReportModel : BaseFrmReportModel
    {
        public string PMOrgName { get; set; }

        public int? DevolvedUKPRN { get; set; }

        public string DevolvedOrgName { get; set; }

        public string SOFCode { get; set; }

        public string PrevOrgName { get; set; }

        public int AimTypeCode { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public int? WithdrawalCode { get; set; }

        public int FundModel { get; set; }

        public string LearnAimType { get; set; }
    }
}
