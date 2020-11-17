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
        }

        protected override void RegisterRenderService(ContainerBuilder builder)
            => builder.RegisterType<Frm06RenderService>().As<IRenderService<IEnumerable<Frm06ReportModel>>>();

        protected override void RegisterServices(ContainerBuilder builder)
        {
        }
    }
}
