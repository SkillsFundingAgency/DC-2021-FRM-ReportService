using System;
using System.Collections.Generic;
using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;
using ESFA.DC.FRM.ReportService.Reports.Model.Worksheets;
using ESFA.DC.FRM.ReportService.Reports.Worksheets.FRM07;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public class Frm07WorksheetModule : AbstractWorksheetModule<Frm07Worksheet>
    {
        protected override void RegisterModelBuilder(ContainerBuilder builder)
        {
            builder.RegisterType<Frm07ModelBuilder>().As<IModelBuilder<IEnumerable<Frm07ReportModel>>>();
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
        {
            builder.RegisterType<Frm07RenderService>().As<IRenderService<IEnumerable<Frm07ReportModel>>>();
        }

        protected override void RegisterServices(ContainerBuilder builder)
        {
        }
    }
}
