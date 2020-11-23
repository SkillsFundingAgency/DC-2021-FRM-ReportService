using System.Collections.Generic;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM06;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class Frm06WorksheetModule : AbstractWorksheetModule<Frm06Worksheet>
    {
        protected override void RegisterModelBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Frm06ModelBuilder>().As<IModelBuilder<IEnumerable<Frm06ReportModel>>>().InstancePerLifetimeScope();
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
        {
            builder.RegisterType<Frm06RenderService>().As<IRenderService<IEnumerable<Frm06ReportModel>>>().InstancePerLifetimeScope();
        }

        protected override void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<LearnerComparer>().As<IEqualityComparer<LearnerKey>>();
        }
    }
}