using Autofac;
using ESFA.DC.FRM.ReportService.Interfaces;

namespace ESFA.DC.FRM.ReportService.Modules.Worksheets
{
    public abstract class AbstractWorksheetModule<T> : Module
        where T : IWorksheetReport
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<T>().As<IWorksheetReport>().InstancePerLifetimeScope();

            RegisterModelBuilder(builder);
            RegisterRenderService(builder);
        }

        protected abstract void RegisterModelBuilder(ContainerBuilder builder);

        protected abstract void RegisterRenderService(ContainerBuilder builder);
    }
}
