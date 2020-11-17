using System.Collections.Generic;

namespace ESFA.DC.FRM.ReportService.Interfaces.ReferenceData
{
    public interface ILARSLearningDelivery
    {
        string LearnAimRef { get; }

        string LearnAimRefTitle { get; }

        string LearnAimRefTypeDesc { get; }

//        string NotionalNVQLevel { get; set; }
//
//        string NotionalNVQLevelv2 { get; set; }
//
//        int? FrameworkCommonComponent { get; set; }
//
//        Decimal? SectorSubjectAreaTier2 { get; set; }
//
//        string SectorSubjectAreaTier2Desc { get; set; }
//
          IReadOnlyCollection<ILARSLearningDeliveryCategory> LARSLearningDeliveryCategories { get; }
//
//        IReadOnlyCollection<LARSFramework> LARSFrameworks { get; set; }
    }
}
