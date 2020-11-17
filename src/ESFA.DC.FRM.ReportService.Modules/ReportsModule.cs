using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces.Reports;
using ESFA.DC.FRM.ReportService.Modules.Worksheets;
using ESFA.DC.FRM.ReportService.Reports;

namespace ESFA.DC.FRM.ReportService.Modules
{
    public class ReportsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Frm>().As<IReport>().InstancePerLifetimeScope();

            builder.RegisterModule<SummaryWorksheetModule>();
            builder.RegisterModule<Frm06WorksheetModule>();
            builder.RegisterModule<Frm07WorksheetModule>();
            builder.RegisterModule<Frm08WorksheetModule>();
        }
    }
}
