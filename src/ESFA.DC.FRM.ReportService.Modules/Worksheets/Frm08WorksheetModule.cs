using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM08;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class Frm08WorksheetModule : AbstractWorksheetModule<Frm08Worksheet>
    {
        protected override void RegisterModelBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Frm08ModelBuilder>().As<IModelBuilder<IEnumerable<Frm08ReportModel>>>().InstancePerLifetimeScope();
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
        {
            builder.RegisterType<Frm08RenderService>().As<IRenderService<IEnumerable<Frm08ReportModel>>>().InstancePerLifetimeScope();
        }
    }
}
