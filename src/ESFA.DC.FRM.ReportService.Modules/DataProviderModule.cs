using Autofac;
using ESFA.DC.FRM.ReportService.Data;
using ESFA.DC.FRM.ReportService.Data.ReferenceData;
using ESFA.DC.FRM.ReportService.Interfaces.DataProvider;
using ESFA.DC.FRM.ReportService.Interfaces.ReferenceData;

namespace ESFA.DC.FRM.ReportService.Modules
{
    public class DataProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<OrganisationDataProvider>().As<IDataProvider<IOrganisation>>().InstancePerLifetimeScope();
            builder.RegisterType<McaGlaLookupProvider>().As<IDataProvider<IMcaGlaSofLookup>>().InstancePerLifetimeScope();
            builder.RegisterType<McaDevolvedContractsProvider>().As<IDataProvider<IMcaDevolvedContract>>().InstancePerLifetimeScope();
            builder.RegisterType<LARSLearningDeliveryProvider>().As<IDataProvider<ILARSLearningDelivery>>().InstancePerLifetimeScope();

            builder.RegisterType<ReportDataProvider>().As<IReportDataProvider>().InstancePerLifetimeScope();
        }
    }
}
