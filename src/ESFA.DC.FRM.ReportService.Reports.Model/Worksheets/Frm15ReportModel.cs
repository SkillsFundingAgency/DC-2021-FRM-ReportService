using System;

namespace ESFA.DC.FRM.ReportService.Reports.Model.Worksheets
{
    public class Frm15ReportModel : BaseFrmReportModel
    {
        public string EPAOrgId { get; set; }

        public int? TotalNegotiatedAssessmentPrice { get; set; }

        public int? AssessmentPaymentReceived { get; set; }

        public string LearnAimType { get; set; }

        public int FundModel { get; set; }

        public DateTime? OrigLearnStartDate { get; set; }

        public string SOFCode { get; set; }

        public int AimTypeCode { get; set; }
    }
}
