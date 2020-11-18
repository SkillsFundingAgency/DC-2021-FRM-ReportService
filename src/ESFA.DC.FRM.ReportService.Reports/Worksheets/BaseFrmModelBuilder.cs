using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESFA.DC.FRM.ReportService.Reports.Extensions;
using ESFA.DC.ILR.Model.Interface;
using ESFA.DC.ILR2021.DataStore.EF;

namespace ESFA.DC.FRM.ReportService.Reports.Worksheets
{
    public abstract class BaseFrmModelBuilder
    {
        protected const string ADLLearnDelFamType = "ADL";
        protected const string SOFLearnDelFamType = "SOF";
        protected const string RESLearnDelFamType = "RES";

        protected readonly HashSet<string> DevolvedCodes = new HashSet<string> { "110", "111", "112", "113", "114", "115", "116", "117" };

//        protected string RetrieveFamCodeForType(IEnumerable<LearningDeliveryFAM> deliveryFams, string learnDelFamType)
//        {
//            return deliveryFams?.FirstOrDefault(f => f.LearnDelFAMType.CaseInsensitiveEquals(learnDelFamType))?.LearnDelFAMCode ?? string.Empty;
//        }

        protected string RetrieveFamCodeForType(IEnumerable<LearningDeliveryFAM> deliveryFams, string learnDelFamType)
        {
            return deliveryFams?.FirstOrDefault(f => f.LearnDelFAMType.CaseInsensitiveEquals(learnDelFamType))?.LearnDelFAMCode ?? string.Empty;
        }

        protected string ProviderSpecDeliveryMonitorings(ICollection<ProviderSpecDeliveryMonitoring> providerSpecDeliveryMonitorings)
        {
            if (providerSpecDeliveryMonitorings == null || !providerSpecDeliveryMonitorings.Any())
            {
                return null;
            }

            return string.Join(";", providerSpecDeliveryMonitorings?.Select(x => x.ProvSpecDelMon));
        }

        protected string ProviderSpecLearningMonitorings(ICollection<ProviderSpecLearnerMonitoring> providerSpecLearnerMonitorings)
        {
            if (providerSpecLearnerMonitorings == null || !providerSpecLearnerMonitorings.Any())
            {
                return null;
            }

            return string.Join(";", providerSpecLearnerMonitorings?.Select(x => x.ProvSpecLearnMon));
        }
    }
}
