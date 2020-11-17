using System;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Worksheets
{
    public class Frm07ReportModel : BaseFrmReportModel
    {
        public string PrevOrgName { get; set; }

        public string PMOrgName { get; set; }

        public int? DevolvedUKPRN { get; set; }

        public string DevolvedOrgName { get; set; }

        public int AimTypeCode { get; set; }

        public string LearnAimType { get; set; }

        public int FundModel { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public string SOFCode { get; set; }
    }
}
