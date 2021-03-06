﻿using System;

namespace ESFA.DC.FRM.ReportService.Reports.Constants
{
    public static class ReportingConstants
    {
        public const string OfficialSensitive = "OFFICIAL-SENSITIVE";
        public const string Yes = "Yes";
        public const string No = "No";

        public const string EmploymentStatusMonitoringTypeSEM = "SEM";

        public const string Y = "Y";
        public const string N = "N";

        // Dates
        public const string Year = "2020/21";
        public const string ShortYearStart = "2020";
        public const string ShortYearEnd = "2021";

        public const string ReferenceDateFilterPropertyName = @"Reference Date";

        public static readonly DateTime BeginningOfYear = new DateTime(2020, 8, 1);
        public static readonly DateTime EndOfYear = new DateTime(2021, 7, 31, 23, 59, 59);
    }
}
