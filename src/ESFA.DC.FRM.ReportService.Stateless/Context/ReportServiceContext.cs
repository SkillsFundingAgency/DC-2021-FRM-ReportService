using System;
using System.Collections.Generic;
using System.Linq;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.ILR.Constants;
using ESFA.DC.JobContextManager.Model.Interface;

namespace ESFA.DC.FRM.ReportService.Stateless.Context
{
    public class ReportServiceContext : IReportServiceContext
    {
        private readonly IJobContextMessage _jobContextMessage;

        public ReportServiceContext(IJobContextMessage jobContextMessage)
        {
            _jobContextMessage = jobContextMessage;
        }

        public long JobId => _jobContextMessage.JobId;

        public string Filename => _jobContextMessage.KeyValuePairs[ILRContextKeys.Filename].ToString();

        public long Ukprn => long.Parse(_jobContextMessage.KeyValuePairs[ILRContextKeys.Ukprn].ToString());

        public string Container => _jobContextMessage.KeyValuePairs[ILRContextKeys.Container].ToString();

        public IEnumerable<string> Tasks => _jobContextMessage.Topics[_jobContextMessage.TopicPointer].Tasks.SelectMany(x => x.Tasks);

        public DateTime SubmissionDateTimeUtc => _jobContextMessage.SubmissionDateTimeUtc;

        public int ReturnPeriod => int.Parse(_jobContextMessage.KeyValuePairs[ILRContextKeys.ReturnPeriod].ToString());

        public string ReturnPeriodName => $"R{ReturnPeriod:D2}";
    }
}
