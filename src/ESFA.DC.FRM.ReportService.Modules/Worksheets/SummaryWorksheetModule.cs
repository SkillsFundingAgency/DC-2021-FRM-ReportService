using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;
using ESFA.DC.FRM.ReportService.Reports.Summary;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class SummaryWorksheetModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FrmSummaryModelBuilder>().As<IModelBuilder<ISummaryModel>>().InstancePerLifetimeScope();
            builder.RegisterType<FrmSummaryRenderService>().As<IRenderService<ISummaryModel>>().InstancePerLifetimeScope();
        }
    }
}
