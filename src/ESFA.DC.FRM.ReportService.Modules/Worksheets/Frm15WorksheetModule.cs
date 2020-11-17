using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM15;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class Frm15WorksheetModule : AbstractWorksheetModule<Frm15Worksheet>
    {
        protected override void RegisterModelBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Frm15ModelBuilder>().As<IModelBuilder<IEnumerable<Frm15ReportModel>>>();
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
        {
            builder.RegisterType<Frm15RenderService>().As<IRenderService<IEnumerable<Frm15ReportModel>>>();
        }

        protected override void RegisterServices(ContainerBuilder builder)
        {
        }
    }
}
