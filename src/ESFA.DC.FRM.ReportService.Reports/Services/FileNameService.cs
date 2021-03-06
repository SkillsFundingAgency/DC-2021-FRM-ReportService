﻿using System.Collections.Generic;
using System.Text;
using ESFA.DC.DateTimeProvider.Interface;
using ESFA.DC.FRM.ReportService.Interfaces;

namespace ESFA.DC.FRM.ReportService.Reports.Services
{
    public class FileNameService : IFileNameService
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        private readonly IDictionary<OutputTypes, string> _extensionsDictionary = new Dictionary<OutputTypes, string>()
        {
            [OutputTypes.Csv] = "csv",
            [OutputTypes.Excel] = "xlsx",
            [OutputTypes.Json] = "json",
            [OutputTypes.Zip] = "zip",
        };

        public FileNameService(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public string GetFilename(IReportServiceContext reportServiceContext, string fileName, OutputTypes outputType, bool includeDateTime = true)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(GetPath(reportServiceContext));

            stringBuilder.Append(fileName);

            if (includeDateTime)
            {
                stringBuilder.Append($" {_dateTimeProvider.ConvertUtcToUk(reportServiceContext.SubmissionDateTimeUtc):yyyyMMdd-HHmmss}");
            }

            stringBuilder.Append($".{GetExtension(outputType)}");

            return stringBuilder.ToString();
        }

        public string GetExtension(OutputTypes outputType) => _extensionsDictionary[outputType];

        protected virtual string GetPath(IReportServiceContext reportServiceContext) => $"{reportServiceContext.Ukprn}/{reportServiceContext.JobId}/";
    }
}
