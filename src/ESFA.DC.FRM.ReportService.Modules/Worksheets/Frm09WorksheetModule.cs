using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM09;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class Frm09WorksheetModule : AbstractWorksheetModule<Frm09Worksheet>
    {
        protected override void RegisterModelBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Frm09ModelBuilder>().As<IModelBuilder<IEnumerable<Frm09ReportModel>>>().InstancePerLifetimeScope();
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
        {
            builder.RegisterType<Frm09RenderService>().As<IRenderService<IEnumerable<Frm09ReportModel>>>().InstancePerLifetimeScope();
        }
    }
}
